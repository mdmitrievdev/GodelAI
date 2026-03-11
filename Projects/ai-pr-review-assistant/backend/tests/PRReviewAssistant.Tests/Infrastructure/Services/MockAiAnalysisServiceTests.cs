using FluentAssertions;
using PRReviewAssistant.API.Domain.Enums;
using PRReviewAssistant.API.Infrastructure.Services;

namespace PRReviewAssistant.Tests.Infrastructure.Services;

public class MockAiAnalysisServiceTests
{
    private readonly MockAiAnalysisService _service = new();

    [Fact]
    public async Task AnalyzeAsync_ValidInput_ReturnsAtLeastThreeFindings()
    {
        // Arrange
        var codeDiff = "+ var x = 1;\n- var y = 2;\n+ var z = 3;";

        // Act
        var findings = await _service.AnalyzeAsync(codeDiff, "C#", CancellationToken.None);

        // Assert
        findings.Should().HaveCountGreaterThanOrEqualTo(3);
    }

    [Fact]
    public async Task AnalyzeAsync_ValidInput_AllFindingsHaveNonEmptyId()
    {
        // Arrange
        var codeDiff = "+ var x = 1;";

        // Act
        var findings = await _service.AnalyzeAsync(codeDiff, "C#", CancellationToken.None);

        // Assert
        findings.Should().AllSatisfy(f => f.Id.Should().NotBeNullOrWhiteSpace());
    }

    [Fact]
    public async Task AnalyzeAsync_ValidInput_AllSeverityLevelsPresent()
    {
        // Arrange
        var codeDiff = string.Join("\n", Enumerable.Range(1, 50).Select(i => $"+ line {i}"));

        // Act
        var findings = await _service.AnalyzeAsync(codeDiff, "C#", CancellationToken.None);

        // Assert
        var severities = findings.Select(f => f.Severity).Distinct().ToList();
        severities.Should().Contain(FindingSeverity.Critical);
        severities.Should().Contain(FindingSeverity.Warning);
        severities.Should().Contain(FindingSeverity.Info);
    }

    [Fact]
    public async Task AnalyzeAsync_ValidInput_ConfidenceWithinRange()
    {
        // Arrange
        var codeDiff = "+ var x = 1;\n+ var y = 2;";

        // Act
        var findings = await _service.AnalyzeAsync(codeDiff, "TypeScript", CancellationToken.None);

        // Assert
        findings.Should().AllSatisfy(f => f.Confidence.Should().BeInRange(0, 100));
    }

    [Fact]
    public async Task AnalyzeAsync_CSharpLanguage_ReturnsFindingsFromValidTemplates()
    {
        // Arrange
        var codeDiff = string.Join("\n", Enumerable.Range(1, 30).Select(i => $"+ line {i}"));

        // Act
        var findings = await _service.AnalyzeAsync(codeDiff, "C#", CancellationToken.None);

        // Assert
        findings.Should().AllSatisfy(f =>
        {
            f.Title.Should().NotBeNullOrWhiteSpace();
            f.Description.Should().NotBeNullOrWhiteSpace();
            f.Suggestion.Should().NotBeNullOrWhiteSpace();
        });
    }

    [Fact]
    public async Task AnalyzeAsync_LongInput_ReturnsMoreFindings()
    {
        // Arrange
        var shortDiff = "+ line 1\n+ line 2";
        var longDiff = string.Join("\n", Enumerable.Range(1, 200).Select(i => $"+ line {i}"));

        // Act
        var shortFindings = await _service.AnalyzeAsync(shortDiff, "C#", CancellationToken.None);
        var longFindings = await _service.AnalyzeAsync(longDiff, "C#", CancellationToken.None);

        // Assert
        longFindings.Count.Should().BeGreaterThanOrEqualTo(shortFindings.Count);
    }

    [Fact]
    public async Task AnalyzeAsync_CancellationRequested_ThrowsOperationCanceled()
    {
        // Arrange
        var codeDiff = "+ var x = 1;";
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        var act = () => _service.AnalyzeAsync(codeDiff, "C#", cts.Token);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }
}
