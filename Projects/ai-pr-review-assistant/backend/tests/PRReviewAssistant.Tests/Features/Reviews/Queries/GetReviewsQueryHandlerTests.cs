using FluentAssertions;
using Moq;
using PRReviewAssistant.API.Domain.Interfaces;
using PRReviewAssistant.API.Features.Reviews.Queries;
using PRReviewAssistant.API.Shared.DTOs;

namespace PRReviewAssistant.Tests.Features.Reviews.Queries;

public class GetReviewsQueryHandlerTests
{
    private readonly Mock<IReviewRepository> _reviewRepositoryMock;
    private readonly GetReviewsQueryHandler _handler;

    public GetReviewsQueryHandlerTests()
    {
        _reviewRepositoryMock = new Mock<IReviewRepository>();
        _handler = new GetReviewsQueryHandler(_reviewRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_DefaultPagination_CallsRepositoryWithDefaults()
    {
        // Arrange
        var query = new GetReviewsQuery();
        _reviewRepositoryMock
            .Setup(r => r.GetAllAsync(1, 20, It.IsAny<CancellationToken>()))
            .ReturnsAsync((new List<ReviewListItem>() as IReadOnlyList<ReviewListItem>, 0));

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _reviewRepositoryMock.Verify(
            r => r.GetAllAsync(1, 20, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_EmptyRepository_ReturnsEmptyPaginatedList()
    {
        // Arrange
        var query = new GetReviewsQuery();
        _reviewRepositoryMock
            .Setup(r => r.GetAllAsync(1, 20, It.IsAny<CancellationToken>()))
            .ReturnsAsync((new List<ReviewListItem>() as IReadOnlyList<ReviewListItem>, 0));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task Handle_WithResults_ReturnsPaginatedList()
    {
        // Arrange
        var items = new List<ReviewListItem>
        {
            new("id-1", DateTime.UtcNow, "C#", "snippet 1", 5, 1, 2, 2),
            new("id-2", DateTime.UtcNow, "Python", "snippet 2", 3, 0, 1, 2)
        };
        var query = new GetReviewsQuery(Page: 1, PageSize: 10);
        _reviewRepositoryMock
            .Setup(r => r.GetAllAsync(1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync((items as IReadOnlyList<ReviewListItem>, 25));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(25);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);
    }

    [Fact]
    public async Task Handle_SpecificPage_PassesPageToRepository()
    {
        // Arrange
        var query = new GetReviewsQuery(Page: 3, PageSize: 5);
        _reviewRepositoryMock
            .Setup(r => r.GetAllAsync(3, 5, It.IsAny<CancellationToken>()))
            .ReturnsAsync((new List<ReviewListItem>() as IReadOnlyList<ReviewListItem>, 50));

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _reviewRepositoryMock.Verify(
            r => r.GetAllAsync(3, 5, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
