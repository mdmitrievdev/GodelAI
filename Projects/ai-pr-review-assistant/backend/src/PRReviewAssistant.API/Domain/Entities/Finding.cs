using PRReviewAssistant.API.Domain.Enums;

namespace PRReviewAssistant.API.Domain.Entities;

public class Finding
{
    public string Id { get; set; } = string.Empty;
    public string ReviewId { get; set; } = string.Empty;
    public FindingCategory Category { get; set; }
    public FindingSeverity Severity { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? LineReference { get; set; }
    public string Suggestion { get; set; } = string.Empty;
    public int Confidence { get; set; }
    public string? SuggestedFix { get; set; }
    public Review Review { get; set; } = null!;
}
