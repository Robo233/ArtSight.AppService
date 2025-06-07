using FluentValidation;
using ArtSight.AppService.Models.Requests;
using ArtSight.AppService.Models.Extensions;

namespace ArtSight.AppService.Models.Validators;

public class EntityActionRequestValidator : AbstractValidator<EntityActionRequest>
{
    public EntityActionRequestValidator()
    {
        RuleFor(x => x.Id).MustBeValidId();

        RuleFor(x => x.EntityType).MustBeValidEntityType();
    }

}
