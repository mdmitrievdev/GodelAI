using MediatR;
using Microsoft.AspNetCore.Mvc;
using PRReviewAssistant.API.Features.Settings.Commands;
using PRReviewAssistant.API.Features.Settings.Queries;
using PRReviewAssistant.API.Shared.DTOs;

namespace PRReviewAssistant.API.Endpoints;

public static class SettingsEndpoints
{
    public static void MapSettingsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/settings")
            .WithTags("Settings");

        group.MapGet("/", GetSettings)
            .WithName("GetSettings")
            .WithSummary("Get application settings");

        group.MapPut("/", UpdateSettings)
            .WithName("UpdateSettings")
            .WithSummary("Update application settings");
    }

    private static async Task<IResult> GetSettings(
        IMediator mediator,
        CancellationToken ct)
    {
        var query = new GetSettingsQuery();
        var result = await mediator.Send(query, ct);
        return Results.Ok(ApiResponse<AppSettingsResponse>.Success(result));
    }

    private static async Task<IResult> UpdateSettings(
        [FromBody] UpdateSettingsRequest request,
        IMediator mediator,
        CancellationToken ct)
    {
        var command = new UpdateSettingsCommand(request.UseMockAi, request.AiModel);
        var result = await mediator.Send(command, ct);
        return Results.Ok(ApiResponse<AppSettingsResponse>.Success(result));
    }
}
