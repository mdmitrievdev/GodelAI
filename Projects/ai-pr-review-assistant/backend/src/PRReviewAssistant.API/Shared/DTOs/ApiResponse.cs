using Microsoft.AspNetCore.Mvc;

namespace PRReviewAssistant.API.Shared.DTOs;

public record ApiResponse<T>
{
    public T? Data { get; init; }
    public ProblemDetails? Error { get; init; }

    public static ApiResponse<T> Success(T data) => new() { Data = data };

    public static ApiResponse<T> Failure(ProblemDetails error) => new() { Error = error };
}
