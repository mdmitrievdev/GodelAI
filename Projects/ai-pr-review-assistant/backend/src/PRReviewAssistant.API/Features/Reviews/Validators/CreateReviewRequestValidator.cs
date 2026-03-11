using FluentValidation;
using PRReviewAssistant.API.Shared.DTOs;

namespace PRReviewAssistant.API.Features.Reviews.Validators;

public class CreateReviewRequestValidator : AbstractValidator<CreateReviewRequest>
{
    private static readonly IReadOnlyList<string> SupportedLanguages =
    [
        "C#", "TypeScript", "JavaScript", "Python", "Java", "Go", "Other"
    ];

    public CreateReviewRequestValidator()
    {
        RuleFor(x => x.CodeDiff)
            .NotEmpty()
                .WithMessage("Code diff is required.")
            .MaximumLength(50_000)
                .WithMessage("Code diff must not exceed 50,000 characters.");

        RuleFor(x => x.Language)
            .NotEmpty()
                .WithMessage("Language is required.")
            .Must(lang => SupportedLanguages.Contains(lang))
                .WithMessage($"Language must be one of: {string.Join(", ", SupportedLanguages)}.");

        RuleFor(x => x.PrUrl)
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out var uri)
                         && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                .WithMessage("PR URL must be a valid HTTP or HTTPS URL.")
            .When(x => !string.IsNullOrEmpty(x.PrUrl));
    }
}
