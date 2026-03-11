using MediatR;
using PRReviewAssistant.API.Domain.Interfaces;

namespace PRReviewAssistant.API.Features.Reviews.Commands;

public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, bool>
{
    private readonly IReviewRepository _reviewRepository;

    public DeleteReviewCommandHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        return await _reviewRepository.DeleteAsync(request.Id, cancellationToken);
    }
}
