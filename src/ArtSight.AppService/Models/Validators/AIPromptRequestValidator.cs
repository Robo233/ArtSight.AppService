using ArtSight.AppService.Models.Extensions;
using ArtSight.AppService.Models.Requests;
using FluentValidation;

namespace ArtSight.AppService.Models.Validators;

public class AIPromptRequestValidator : AbstractValidator<AIPromptRequest>
{
    public AIPromptRequestValidator()
    {
        RuleFor(x => x.Prompt)
            .NotEmpty().WithMessage("Prompt is required.")
            .MaximumLength(500).WithMessage("Prompt must not exceed 500 characters.");

        RuleFor(x => x.ConversationToken)
            .NotEmpty().WithMessage("Conversation token is required.");

        RuleFor(x => x.LanguageCode).MustBeValidLanguageCode();
    }
}
