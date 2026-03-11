using MediatR;
using Microsoft.AspNetCore.Mvc;
using PRReviewAssistant.API.Features.Reviews.Commands;
using PRReviewAssistant.API.Features.Reviews.Queries;
using PRReviewAssistant.API.Shared.DTOs;

namespace PRReviewAssistant.API.Endpoints;

public static class ReviewEndpoints
{
    public static void MapReviewEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/reviews")
            .WithTags("Reviews");

        group.MapPost("/", CreateReview)
            .WithName("CreateReview")
            .WithSummary("Submit a code diff for AI analysis");

        group.MapGet("/", GetReviews)
            .WithName("GetReviews")
            .WithSummary("Get paginated list of reviews");

        group.MapGet("/{id}", GetReviewById)
            .WithName("GetReviewById")
            .WithSummary("Get a review with all findings by ID");

        group.MapDelete("/{id}", DeleteReview)
            .WithName("DeleteReview")
            .WithSummary("Delete a review and its findings");
    }

    private static async Task<IResult> CreateReview(
        [FromBody] CreateReviewRequest request,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new CreateReviewCommand(request.CodeDiff, request.Language, request.PrUrl);
        var result = await mediator.Send(command, ct);
        return Results.Created($"/api/v1/reviews/{result.Id}", ApiResponse<ReviewDetailResponse>.Success(result));
    }

    private static async Task<IResult> GetReviews(
        IMediator mediator,
        CancellationToken ct,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = new GetReviewsQuery(page, pageSize);
        var result = await mediator.Send(query, ct);
        return Results.Ok(ApiResponse<PaginatedList<ReviewListItem>>.Success(result));
    }

    private static async Task<IResult> GetReviewById(
        string id,
        IMediator mediator,
        CancellationToken ct)
    {
        var query = new GetReviewByIdQuery(id);
        var result = await mediator.Send(query, ct);

        if (result is null)
        {
            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Review not found",
                Detail = $"No review with ID '{id}' exists."
            };
            return Results.NotFound(ApiResponse<ReviewDetailResponse>.Failure(problem));
        }

        return Results.Ok(ApiResponse<ReviewDetailResponse>.Success(result));
    }

    private static async Task<IResult> DeleteReview(
        string id,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new DeleteReviewCommand(id);
        var deleted = await mediator.Send(command, ct);

        if (!deleted)
        {
            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Review not found",
                Detail = $"No review with ID '{id}' exists."
            };
            return Results.NotFound(ApiResponse<bool>.Failure(problem));
        }

        return Results.NoContent();
    }
}
