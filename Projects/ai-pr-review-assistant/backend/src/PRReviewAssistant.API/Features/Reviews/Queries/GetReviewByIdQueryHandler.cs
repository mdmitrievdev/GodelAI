using MediatR;
using PRReviewAssistant.API.Domain.Enums;
using PRReviewAssistant.API.Domain.Interfaces;
using PRReviewAssistant.API.Shared.DTOs;

namespace PRReviewAssistant.API.Features.Reviews.Queries;

public class GetReviewByIdQueryHandler : IRequestHandler<GetReviewByIdQuery, ReviewDetailResponse?>
{
    private readonly IReviewRepository _reviewRepository;

    public GetReviewByIdQueryHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<ReviewDetailResponse?> Handle(
        GetReviewByIdQuery request, CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByIdAsync(request.Id, cancellationToken);

        if (review is null)
            return null;

        var findingDtos = review.Findings.Select(f => new FindingDto(
            f.Id,
            f.Category.ToString(),
            f.Severity.ToString(),
            f.Title,
            f.Description,
            f.LineReference,
            f.Suggestion,
            f.Confidence,
            f.SuggestedFix)).ToList();

        var summary = new ReviewSummaryDto(
            TotalFindings: findingDtos.Count,
            CriticalCount: review.Findings.Count(f => f.Severity == FindingSeverity.Critical),
            WarningCount: review.Findings.Count(f => f.Severity == FindingSeverity.Warning),
            InfoCount: review.Findings.Count(f => f.Severity == FindingSeverity.Info),
            AverageConfidence: review.Findings.Count > 0
                ? Math.Round(review.Findings.Average(f => f.Confidence), 1)
                : 0);

        return new ReviewDetailResponse(
            review.Id,
            review.CreatedAt,
            review.Language,
            review.CodeDiff,
            review.PrUrl,
            summary,
            findingDtos);
    }
}
