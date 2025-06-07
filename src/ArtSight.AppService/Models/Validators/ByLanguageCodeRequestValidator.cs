using FluentValidation;
using ArtSight.AppService.Models.Requests;
using ArtSight.AppService.Models.Extensions;

namespace ArtSight.AppService.Models.Validators;

public class GetAllExhibitionsRequestValidator : AbstractValidator<ByLanguageCodeRequest>
{
    public GetAllExhibitionsRequestValidator()
    {
        RuleFor(x => x.LanguageCode).MustBeValidLanguageCode();
    }
}
