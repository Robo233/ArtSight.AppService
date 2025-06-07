using ArtSight.AppService.Models.Extensions;
using ArtSight.AppService.Models.Requests;
using FluentValidation;

namespace ArtSight.AppService.Models.Validators;

public class EntityFilterRequestValidator : AbstractValidator<EntityFilterRequest>
{
    public EntityFilterRequestValidator()
    {
        Include(new BaseEntityFilterRequestValidator());

        RuleFor(x => x.EntityType).MustBeValidEntityType();

        RuleFor(x => x.Offset)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Offset must be a non-negative value.");
    }
}
