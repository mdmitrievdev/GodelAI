namespace PRReviewAssistant.API.Shared.DTOs;

public record ReviewListItem(
    string Id,
    DateTime CreatedAt,
    string Language,
    string CodeSnippet,
    int TotalFindings,
    int CriticalCount,
    int WarningCount,
    int InfoCount);
