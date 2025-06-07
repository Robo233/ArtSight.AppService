using FluentValidation;
using ArtSight.AppService.Models.Requests;
using ArtSight.AppService.Models.Extensions;

namespace ArtSight.AppService.Models.Validators;

public class BaseEntityFilterRequestValidator : AbstractValidator<BaseEntityFilterRequest>
{
    public BaseEntityFilterRequestValidator()
    {
        RuleFor(x => x.SearchString)
            .MaximumLength(100)
            .WithMessage("SearchString must be 100 characters or fewer.")
            .When(x => !string.IsNullOrEmpty(x.SearchString));

        RuleFor(x => x.IsFavorited)
            .NotNull()
            .When(x => x.IsFavorited.HasValue)
            .WithMessage("IsFavorited must be a valid boolean value.");

        RuleFor(x => x.IsCreatedByUser)
            .NotNull()
            .When(x => x.IsCreatedByUser.HasValue)
            .WithMessage("IsCreatedByUser must be a valid boolean value.");

        RuleFor(x => x.IsRecentlyViewed)
            .NotNull()
            .When(x => x.IsRecentlyViewed.HasValue)
            .WithMessage("IsRecentlyViewed must be a valid boolean value.");

        RuleFor(x => x.LanguageCode).MustBeValidLanguageCode();

    }
}
