namespace PRReviewAssistant.API.Shared.DTOs;

public record ReviewDetailResponse(
    string Id,
    DateTime CreatedAt,
    string Language,
    string CodeDiff,
    string? PrUrl,
    ReviewSummaryDto Summary,
    IReadOnlyList<FindingDto> Findings);
