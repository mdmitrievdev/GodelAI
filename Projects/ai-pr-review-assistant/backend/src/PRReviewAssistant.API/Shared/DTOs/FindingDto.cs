namespace PRReviewAssistant.API.Shared.DTOs;

public record FindingDto(
    string Id,
    string Category,
    string Severity,
    string Title,
    string Description,
    string? LineReference,
    string Suggestion,
    int Confidence,
    string? SuggestedFix);
