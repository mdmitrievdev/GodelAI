using MediatR;

namespace PRReviewAssistant.API.Features.Reviews.Commands;

public record DeleteReviewCommand(string Id) : IRequest<bool>;
