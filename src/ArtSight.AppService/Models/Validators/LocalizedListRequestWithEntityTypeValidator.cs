using FluentValidation;
using ArtSight.AppService.Models.Requests;
using ArtSight.AppService.Models.Extensions;

namespace ArtSight.AppService.Models.Validators;

public class LocalizedListRequestWithEntityTypeValidator : AbstractValidator<LocalizedListRequestWithEntityType>
{
    public LocalizedListRequestWithEntityTypeValidator()
    {
        RuleFor(x => x.Offset)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Offset must be greater than or equal to 0.");

        RuleFor(x => x.Limit)
            .GreaterThan(0)
            .WithMessage("Limit must be greater than 0.");

        RuleFor(x => x.EntityType).MustBeValidEntityType();

        RuleFor(x => x.LanguageCode).MustBeValidLanguageCode();
    }
}
