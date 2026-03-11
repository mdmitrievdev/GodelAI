using MediatR;
using PRReviewAssistant.API.Shared.DTOs;

namespace PRReviewAssistant.API.Features.Settings.Commands;

public record UpdateSettingsCommand(
    bool UseMockAi,
    string AiModel) : IRequest<AppSettingsResponse>;
