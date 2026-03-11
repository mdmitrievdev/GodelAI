namespace PRReviewAssistant.API.Shared.DTOs;

public record PaginatedList<T>(
    IReadOnlyList<T> Items,
    int TotalCount,
    int Page,
    int PageSize);
