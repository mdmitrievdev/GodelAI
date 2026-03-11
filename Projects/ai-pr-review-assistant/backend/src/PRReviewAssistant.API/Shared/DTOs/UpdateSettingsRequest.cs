namespace PRReviewAssistant.API.Shared.DTOs;

public record UpdateSettingsRequest(
    bool UseMockAi,
    string AiModel);
