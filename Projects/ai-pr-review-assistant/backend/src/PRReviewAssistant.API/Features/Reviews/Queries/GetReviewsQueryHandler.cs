using MediatR;
using PRReviewAssistant.API.Domain.Interfaces;
using PRReviewAssistant.API.Shared.DTOs;

namespace PRReviewAssistant.API.Features.Reviews.Queries;

public class GetReviewsQueryHandler
    : IRequestHandler<GetReviewsQuery, PaginatedList<ReviewListItem>>
{
    private readonly IReviewRepository _reviewRepository;

    public GetReviewsQueryHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<PaginatedList<ReviewListItem>> Handle(
        GetReviewsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _reviewRepository.GetAllAsync(
            request.Page, request.PageSize, cancellationToken);

        return new PaginatedList<ReviewListItem>(items, totalCount, request.Page, request.PageSize);
    }
}
