using MediatR;
using PRReviewAssistant.API.Domain.Interfaces;
using PRReviewAssistant.API.Shared.DTOs;

namespace PRReviewAssistant.API.Features.Settings.Commands;

public class UpdateSettingsCommandHandler : IRequestHandler<UpdateSettingsCommand, AppSettingsResponse>
{
    private readonly ISettingsRepository _settingsRepository;

    public UpdateSettingsCommandHandler(ISettingsRepository settingsRepository)
    {
        _settingsRepository = settingsRepository;
    }

    public async Task<AppSettingsResponse> Handle(
        UpdateSettingsCommand request, CancellationToken cancellationToken)
    {
        var settings = await _settingsRepository.GetAsync(cancellationToken);

        settings.UseMockAi = request.UseMockAi;
        settings.AiModel = request.AiModel;

        var updated = await _settingsRepository.UpdateAsync(settings, cancellationToken);
        return new AppSettingsResponse(updated.UseMockAi, updated.AiModel);
    }
}
