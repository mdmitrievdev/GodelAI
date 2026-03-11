using MediatR;
using PRReviewAssistant.API.Shared.DTOs;

namespace PRReviewAssistant.API.Features.Reviews.Commands;

public record CreateReviewCommand(
    string CodeDiff,
    string Language,
    string? PrUrl) : IRequest<ReviewDetailResponse>;
