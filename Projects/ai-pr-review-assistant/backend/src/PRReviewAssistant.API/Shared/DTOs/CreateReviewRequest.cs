namespace PRReviewAssistant.API.Shared.DTOs;

public record CreateReviewRequest(
    string CodeDiff,
    string Language,
    string? PrUrl);
