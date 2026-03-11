namespace PRReviewAssistant.API.Domain.Entities;

public class AppSettings
{
    public string Id { get; set; } = string.Empty;
    public bool UseMockAi { get; set; } = true;
    public string AiModel { get; set; } = "mock";
}
