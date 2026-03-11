using Microsoft.EntityFrameworkCore;
using PRReviewAssistant.API.Domain.Entities;
using PRReviewAssistant.API.Domain.Interfaces;
using PRReviewAssistant.API.Infrastructure.Data;

namespace PRReviewAssistant.API.Infrastructure.Repositories;

public class SettingsRepository : ISettingsRepository
{
    private const string DefaultId = "default";
    private readonly AppDbContext _db;

    public SettingsRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<AppSettings> GetAsync(CancellationToken ct)
    {
        var settings = await _db.AppSettings
            .FirstOrDefaultAsync(s => s.Id == DefaultId, ct);

        if (settings is not null)
            return settings;

        settings = new AppSettings
        {
            Id = DefaultId,
            UseMockAi = true,
            AiModel = "mock"
        };

        _db.AppSettings.Add(settings);
        await _db.SaveChangesAsync(ct);
        return settings;
    }

    public async Task<AppSettings> UpdateAsync(AppSettings settings, CancellationToken ct)
    {
        var existing = await _db.AppSettings
            .FirstOrDefaultAsync(s => s.Id == DefaultId, ct);

        if (existing is null)
        {
            settings.Id = DefaultId;
            _db.AppSettings.Add(settings);
        }
        else
        {
            existing.UseMockAi = settings.UseMockAi;
            existing.AiModel = settings.AiModel;
        }

        await _db.SaveChangesAsync(ct);
        return existing ?? settings;
    }
}
