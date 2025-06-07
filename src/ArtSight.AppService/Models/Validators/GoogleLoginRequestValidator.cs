using FluentValidation;
using ArtSight.AppService.Models.Requests;

namespace ArtSight.AppService.Models.Validators;

public class GoogleLoginRequestValidator : AbstractValidator<GoogleLoginRequest>
{
    public GoogleLoginRequestValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token is required.");
    }

}
