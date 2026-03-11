using MediatR;
using PRReviewAssistant.API.Shared.DTOs;

namespace PRReviewAssistant.API.Features.Reviews.Queries;

public record GetReviewsQuery(int Page = 1, int PageSize = 20)
    : IRequest<PaginatedList<ReviewListItem>>;
