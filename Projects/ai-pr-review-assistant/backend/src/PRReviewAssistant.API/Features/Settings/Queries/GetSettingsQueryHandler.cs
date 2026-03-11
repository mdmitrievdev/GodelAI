using MediatR;
using PRReviewAssistant.API.Domain.Interfaces;
using PRReviewAssistant.API.Shared.DTOs;

namespace PRReviewAssistant.API.Features.Settings.Queries;

public class GetSettingsQueryHandler : IRequestHandler<GetSettingsQuery, AppSettingsResponse>
{
    private readonly ISettingsRepository _settingsRepository;

    public GetSettingsQueryHandler(ISettingsRepository settingsRepository)
    {
        _settingsRepository = settingsRepository;
    }

    public async Task<AppSettingsResponse> Handle(
        GetSettingsQuery request, CancellationToken cancellationToken)
    {
        var settings = await _settingsRepository.GetAsync(cancellationToken);
        return new AppSettingsResponse(settings.UseMockAi, settings.AiModel);
    }
}
