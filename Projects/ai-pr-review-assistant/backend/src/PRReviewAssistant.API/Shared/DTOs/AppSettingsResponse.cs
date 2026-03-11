namespace PRReviewAssistant.API.Shared.DTOs;

public record AppSettingsResponse(
    bool UseMockAi,
    string AiModel);
