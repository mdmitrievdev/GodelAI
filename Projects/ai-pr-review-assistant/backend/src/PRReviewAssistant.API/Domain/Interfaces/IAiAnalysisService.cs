using PRReviewAssistant.API.Domain.Entities;

namespace PRReviewAssistant.API.Domain.Interfaces;

/// <summary>
/// Service interface for AI-powered code diff analysis.
/// </summary>
public interface IAiAnalysisService
{
    /// <summary>
    /// Analyzes the supplied code diff and returns a list of structured findings.
    /// </summary>
    /// <param name="codeDiff">The raw code diff text to analyze.</param>
    /// <param name="language">The programming language of the diff (e.g. "C#", "TypeScript").</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A read-only list of <see cref="Finding"/> objects produced by the analysis.</returns>
    Task<IReadOnlyList<Finding>> AnalyzeAsync(string codeDiff, string language, CancellationToken ct);
}
