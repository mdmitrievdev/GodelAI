using PRReviewAssistant.API.Domain.Entities;
using PRReviewAssistant.API.Shared.DTOs;

namespace PRReviewAssistant.API.Domain.Interfaces;

/// <summary>
/// Repository interface for managing <see cref="Review"/> entities.
/// </summary>
public interface IReviewRepository
{
    /// <summary>
    /// Persists a new review to the data store.
    /// </summary>
    /// <param name="review">The review entity to create.</param>
    /// <param name="ct">Cancellation token.</param>
    Task CreateAsync(Review review, CancellationToken ct);

    /// <summary>
    /// Retrieves a single review by its identifier, including its findings.
    /// Returns <c>null</c> if no review matches the given <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The ULID string identifier of the review.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<Review?> GetByIdAsync(string id, CancellationToken ct);

    /// <summary>
    /// Returns a paginated projection of all reviews sorted by creation date (newest first).
    /// </summary>
    /// <param name="page">1-based page number.</param>
    /// <param name="pageSize">Number of items per page.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A tuple of the projected items and the total count of all reviews.</returns>
    Task<(IReadOnlyList<ReviewListItem> Items, int TotalCount)> GetAllAsync(
        int page, int pageSize, CancellationToken ct);

    /// <summary>
    /// Deletes the review with the specified identifier, cascading to its findings.
    /// Returns <c>true</c> if the review was found and deleted; <c>false</c> otherwise.
    /// </summary>
    /// <param name="id">The ULID string identifier of the review to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<bool> DeleteAsync(string id, CancellationToken ct);
}
