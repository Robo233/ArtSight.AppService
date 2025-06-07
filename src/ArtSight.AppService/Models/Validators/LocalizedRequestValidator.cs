using FluentValidation;
using ArtSight.AppService.Models.Requests;
using ArtSight.AppService.Models.Extensions;

namespace ArtSight.AppService.Models.Validators;

public class LocalizedRequestValidator : AbstractValidator<LocalizedRequest>
{
    public LocalizedRequestValidator()
    {
        RuleFor(x => x.Id).MustBeValidId();

        RuleFor(x => x.LanguageCode).MustBeValidLanguageCode();
    }
}
