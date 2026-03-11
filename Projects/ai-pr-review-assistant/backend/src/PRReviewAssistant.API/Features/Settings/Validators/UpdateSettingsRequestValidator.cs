using FluentValidation;
using PRReviewAssistant.API.Shared.DTOs;

namespace PRReviewAssistant.API.Features.Settings.Validators;

public class UpdateSettingsRequestValidator : AbstractValidator<UpdateSettingsRequest>
{
    public UpdateSettingsRequestValidator()
    {
        RuleFor(x => x.AiModel)
            .NotEmpty()
                .WithMessage("AI model is required.")
            .MaximumLength(100)
                .WithMessage("AI model must not exceed 100 characters.");
    }
}
