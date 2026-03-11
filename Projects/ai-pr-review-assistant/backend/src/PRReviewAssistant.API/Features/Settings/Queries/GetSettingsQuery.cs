using MediatR;
using PRReviewAssistant.API.Shared.DTOs;

namespace PRReviewAssistant.API.Features.Settings.Queries;

public record GetSettingsQuery : IRequest<AppSettingsResponse>;
