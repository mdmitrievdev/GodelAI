using MediatR;
using PRReviewAssistant.API.Shared.DTOs;

namespace PRReviewAssistant.API.Features.Reviews.Queries;

public record GetReviewByIdQuery(string Id) : IRequest<ReviewDetailResponse?>;
