using MediatR;
using NUlid;
using PRReviewAssistant.API.Domain.Entities;
using PRReviewAssistant.API.Domain.Enums;
using PRReviewAssistant.API.Domain.Interfaces;
using PRReviewAssistant.API.Shared.DTOs;

namespace PRReviewAssistant.API.Features.Reviews.Commands;

public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ReviewDetailResponse>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IAiAnalysisService _aiAnalysisService;

    public CreateReviewCommandHandler(
        IReviewRepository reviewRepository,
        IAiAnalysisService aiAnalysisService)
    {
        _reviewRepository = reviewRepository;
        _aiAnalysisService = aiAnalysisService;
    }

    public async Task<ReviewDetailResponse> Handle(
        CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var findings = await _aiAnalysisService.AnalyzeAsync(
            request.CodeDiff, request.Language, cancellationToken);

        var review = new Review
        {
            Id = Ulid.NewUlid().ToString(),
            CodeDiff = request.CodeDiff,
            Language = request.Language,
            PrUrl = request.PrUrl,
            CreatedAt = DateTime.UtcNow,
            Findings = findings.Select(f => new Finding
            {
                Id = f.Id,
                Category = f.Category,
                Severity = f.Severity,
                Title = f.Title,
                Description = f.Description,
                LineReference = f.LineReference,
                Suggestion = f.Suggestion,
                Confidence = f.Confidence,
                SuggestedFix = f.SuggestedFix
            }).ToList()
        };

        await _reviewRepository.CreateAsync(review, cancellationToken);

        return MapToResponse(review);
    }

    private static ReviewDetailResponse MapToResponse(Review review)
    {
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
