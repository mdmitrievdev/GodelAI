using PRReviewAssistant.API.Domain.Entities;

namespace PRReviewAssistant.API.Domain.Interfaces;

/// <summary>
/// Repository interface for managing the singleton <see cref="AppSettings"/> entity.
/// </summary>
public interface ISettingsRepository
{
    /// <summary>
    /// Retrieves the application settings.
    /// Creates and persists default settings if none exist.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    Task<AppSettings> GetAsync(CancellationToken ct);

    /// <summary>
    /// Persists updated application settings to the data store.
    /// </summary>
    /// <param name="settings">The updated <see cref="AppSettings"/> entity.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<AppSettings> UpdateAsync(AppSettings settings, CancellationToken ct);
}
