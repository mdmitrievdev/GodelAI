namespace PRReviewAssistant.API.Shared.DTOs;

public record ReviewSummaryDto(
    int TotalFindings,
    int CriticalCount,
    int WarningCount,
    int InfoCount,
    double AverageConfidence);
