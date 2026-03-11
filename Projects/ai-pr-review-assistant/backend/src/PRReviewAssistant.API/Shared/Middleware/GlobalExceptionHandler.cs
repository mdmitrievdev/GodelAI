using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace PRReviewAssistant.API.Shared.Middleware;

/// <summary>
/// Catches unhandled exceptions and maps them to RFC 7807 ProblemDetails responses.
/// Registered via <c>builder.Services.AddExceptionHandler&lt;GlobalExceptionHandler&gt;()</c>.
/// </summary>
internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, problemDetails) = exception switch
        {
            ValidationException ve      => MapValidationException(ve),
            KeyNotFoundException        => (StatusCodes.Status404NotFound, new ProblemDetails
            {
                Title    = "Resource Not Found",
                Detail   = exception.Message,
                Status   = StatusCodes.Status404NotFound,
                Type     = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
            }),
            _                           => MapUnhandledException(exception)
        };

        if (statusCode >= StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(
                exception,
                "Unhandled exception of type {ExceptionType}: {Message}",
                exception.GetType().Name,
                exception.Message);
        }
        else
        {
            _logger.LogWarning(
                "Handled exception of type {ExceptionType} mapped to {StatusCode}: {Message}",
                exception.GetType().Name,
                statusCode,
                exception.Message);
        }

        problemDetails.Status ??= statusCode;
        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static (int StatusCode, ProblemDetails ProblemDetails) MapValidationException(
        ValidationException exception)
    {
        var errors = exception.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        var problemDetails = new ProblemDetails
        {
            Title  = "Validation Failed",
            Detail = "One or more validation errors occurred.",
            Status = StatusCodes.Status400BadRequest,
            Type   = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };
        problemDetails.Extensions["errors"] = errors;

        return (StatusCodes.Status400BadRequest, problemDetails);
    }

    private static (int StatusCode, ProblemDetails ProblemDetails) MapUnhandledException(
        Exception exception)
    {
        _ = exception; // intentionally not included in the response — no stack traces exposed

        var problemDetails = new ProblemDetails
        {
            Title  = "An unexpected error occurred",
            Detail = "An internal server error occurred. Please try again later.",
            Status = StatusCodes.Status500InternalServerError,
            Type   = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };

        return (StatusCodes.Status500InternalServerError, problemDetails);
    }
}
