using FluentAssertions;
using Moq;
using PRReviewAssistant.API.Domain.Entities;
using PRReviewAssistant.API.Domain.Enums;
using PRReviewAssistant.API.Domain.Interfaces;
using PRReviewAssistant.API.Features.Reviews.Commands;

namespace PRReviewAssistant.Tests.Features.Reviews.Commands;

public class CreateReviewCommandHandlerTests
{
    private readonly Mock<IReviewRepository> _reviewRepositoryMock;
    private readonly Mock<IAiAnalysisService> _aiAnalysisServiceMock;
    private readonly CreateReviewCommandHandler _handler;

    public CreateReviewCommandHandlerTests()
    {
        _reviewRepositoryMock = new Mock<IReviewRepository>();
        _aiAnalysisServiceMock = new Mock<IAiAnalysisService>();
        _handler = new CreateReviewCommandHandler(
            _reviewRepositoryMock.Object,
            _aiAnalysisServiceMock.Object);
    }

    private static List<Finding> CreateSampleFindings()
    {
        return
        [
            new Finding
            {
                Id = "finding-1",
                Category = FindingCategory.Bug,
                Severity = FindingSeverity.Critical,
                Title = "Null reference",
                Description = "Possible null dereference",
                Suggestion = "Add null check",
                Confidence = 85,
                SuggestedFix = "if (x != null) { ... }"
            },
            new Finding
            {
                Id = "finding-2",
                Category = FindingCategory.Performance,
                Severity = FindingSeverity.Warning,
                Title = "Slow loop",
                Description = "O(n²) complexity",
                Suggestion = "Use a HashSet",
                Confidence = 72,
                LineReference = "Line 10"
            },
            new Finding
            {
                Id = "finding-3",
                Category = FindingCategory.CodeStyle,
                Severity = FindingSeverity.Info,
                Title = "Magic number",
                Description = "Literal value without constant",
                Suggestion = "Extract to constant",
                Confidence = 60
            }
        ];
    }

    private void SetupAiService(string codeDiff, string language, List<Finding> findings)
    {
        _aiAnalysisServiceMock
            .Setup(s => s.AnalyzeAsync(codeDiff, language, It.IsAny<CancellationToken>()))
            .ReturnsAsync(findings);
    }

    [Fact]
    public async Task Handle_ValidInput_ReturnsReviewDetailResponse()
    {
        // Arrange
        var command = new CreateReviewCommand("diff content", "C#", "https://github.com/pr/1");
        var findings = CreateSampleFindings();
        SetupAiService(command.CodeDiff, command.Language, findings);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Language.Should().Be("C#");
        result.CodeDiff.Should().Be("diff content");
        result.PrUrl.Should().Be("https://github.com/pr/1");
    }

    [Fact]
    public async Task Handle_ValidInput_CallsRepositoryCreate()
    {
        // Arrange
        var command = new CreateReviewCommand("diff content", "C#", null);
        SetupAiService(command.CodeDiff, command.Language, CreateSampleFindings());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _reviewRepositoryMock.Verify(
            r => r.CreateAsync(It.IsAny<Review>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ValidInput_CallsAiAnalysisService()
    {
        // Arrange
        var command = new CreateReviewCommand("some diff", "TypeScript", null);
        SetupAiService(command.CodeDiff, command.Language, CreateSampleFindings());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _aiAnalysisServiceMock.Verify(
            s => s.AnalyzeAsync("some diff", "TypeScript", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ValidInput_GeneratesUlidId()
    {
        // Arrange
        var command = new CreateReviewCommand("diff", "Python", null);
        SetupAiService(command.CodeDiff, command.Language, CreateSampleFindings());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Id.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Handle_ValidInput_MapsFindings()
    {
        // Arrange
        var command = new CreateReviewCommand("diff", "C#", null);
        var findings = CreateSampleFindings();
        SetupAiService(command.CodeDiff, command.Language, findings);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Findings.Should().HaveCount(3);
        result.Findings[0].Title.Should().Be("Null reference");
        result.Findings[1].Category.Should().Be("Performance");
        result.Findings[2].Severity.Should().Be("Info");
    }

    [Fact]
    public async Task Handle_WithNullPrUrl_ReturnsNullPrUrl()
    {
        // Arrange
        var command = new CreateReviewCommand("diff", "Go", null);
        SetupAiService(command.CodeDiff, command.Language, CreateSampleFindings());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.PrUrl.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ValidInput_SetsSummary()
    {
        // Arrange
        var command = new CreateReviewCommand("diff", "C#", null);
        var findings = CreateSampleFindings();
        SetupAiService(command.CodeDiff, command.Language, findings);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Summary.TotalFindings.Should().Be(3);
        result.Summary.CriticalCount.Should().Be(1);
        result.Summary.WarningCount.Should().Be(1);
        result.Summary.InfoCount.Should().Be(1);
        result.Summary.AverageConfidence.Should().BeApproximately(72.3, 0.1);
    }
}
