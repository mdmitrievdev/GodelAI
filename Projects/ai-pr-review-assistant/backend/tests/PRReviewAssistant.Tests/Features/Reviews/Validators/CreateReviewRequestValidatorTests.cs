using FluentAssertions;
using PRReviewAssistant.API.Features.Reviews.Validators;
using PRReviewAssistant.API.Shared.DTOs;

namespace PRReviewAssistant.Tests.Features.Reviews.Validators;

public class CreateReviewRequestValidatorTests
{
    private readonly CreateReviewRequestValidator _validator = new();

    [Fact]
    public void Validate_EmptyCodeDiff_ReturnsValidationError()
    {
        // Arrange
        var request = new CreateReviewRequest("", "C#", null);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CodeDiff");
    }

    [Fact]
    public void Validate_CodeDiffExceedsMaxLength_ReturnsValidationError()
    {
        // Arrange
        var longDiff = new string('x', 50_001);
        var request = new CreateReviewRequest(longDiff, "C#", null);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CodeDiff");
    }

    [Fact]
    public void Validate_EmptyLanguage_ReturnsValidationError()
    {
        // Arrange
        var request = new CreateReviewRequest("some diff", "", null);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Language");
    }

    [Fact]
    public void Validate_UnsupportedLanguage_ReturnsValidationError()
    {
        // Arrange
        var request = new CreateReviewRequest("some diff", "Rust", null);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Language");
    }

    [Fact]
    public void Validate_ValidRequest_PassesValidation()
    {
        // Arrange
        var request = new CreateReviewRequest("some diff", "C#", null);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidPrUrl_ReturnsValidationError()
    {
        // Arrange
        var request = new CreateReviewRequest("some diff", "TypeScript", "not-a-url");

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PrUrl");
    }

    [Fact]
    public void Validate_NullPrUrl_PassesValidation()
    {
        // Arrange
        var request = new CreateReviewRequest("some diff", "Python", null);

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
