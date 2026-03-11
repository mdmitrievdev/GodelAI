using PRReviewAssistant.API.Domain.Enums;

namespace PRReviewAssistant.API.Domain.Entities;

public class Review
{
    public string Id { get; set; } = string.Empty;
    public string CodeDiff { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string? PrUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Finding> Findings { get; set; } = [];
}
