using Microsoft.EntityFrameworkCore;
using PRReviewAssistant.API.Domain.Entities;
using PRReviewAssistant.API.Domain.Enums;
using PRReviewAssistant.API.Domain.Interfaces;
using PRReviewAssistant.API.Infrastructure.Data;
using PRReviewAssistant.API.Shared.DTOs;

namespace PRReviewAssistant.API.Infrastructure.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly AppDbContext _db;

    public ReviewRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task CreateAsync(Review review, CancellationToken ct)
    {
        _db.Reviews.Add(review);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<Review?> GetByIdAsync(string id, CancellationToken ct)
    {
        return await _db.Reviews
            .AsNoTracking()
            .Include(r => r.Findings)
            .FirstOrDefaultAsync(r => r.Id == id, ct);
    }

    public async Task<(IReadOnlyList<ReviewListItem> Items, int TotalCount)> GetAllAsync(
        int page, int pageSize, CancellationToken ct)
    {
        var totalCount = await _db.Reviews.CountAsync(ct);

        var items = await _db.Reviews
            .AsNoTracking()
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new ReviewListItem(
                r.Id,
                r.CreatedAt,
                r.Language,
                r.CodeDiff.Length > 80 ? r.CodeDiff.Substring(0, 80) : r.CodeDiff,
                r.Findings.Count,
                r.Findings.Count(f => f.Severity == FindingSeverity.Critical),
                r.Findings.Count(f => f.Severity == FindingSeverity.Warning),
                r.Findings.Count(f => f.Severity == FindingSeverity.Info)))
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken ct)
    {
        var review = await _db.Reviews.FindAsync([id], ct);
        if (review is null)
            return false;

        _db.Reviews.Remove(review);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
