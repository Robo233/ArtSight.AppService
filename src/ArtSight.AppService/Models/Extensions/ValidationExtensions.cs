using FluentValidation;

namespace ArtSight.AppService.Models.Extensions;

public static class ValidationExtensions
{
    private static readonly string[] ValidEntityTypes =
    [
        "artwork",
        "artworkDetail",
        "exhibition",
        "artist",
        "genre"
    ];

    public static IRuleBuilderOptions<T, string?> MustBeValidId<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Id is required.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Invalid Id format.");
    }

    public static IRuleBuilderOptions<T, string?> MustBeValidLanguageCode<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Matches(@"^[a-zA-Z]{2}(-[a-zA-Z]{2})?$")
            .WithMessage("LanguageCode must be a valid ISO language code (e.g., 'en', 'en-US').");
    }
    public static IRuleBuilderOptions<T, string?> MustBeValidEntityType<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("EntityType is required.")
            .Must(entityType => ValidEntityTypes.Contains(entityType, StringComparer.OrdinalIgnoreCase))
            .WithMessage($"Invalid EntityType. Accepted values are: {string.Join(", ", ValidEntityTypes)}.");
    }

    public static IRuleBuilderOptions<T, string?> MustBeValidEmail<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(254).WithMessage("Email must be less than or equal to '254'.");
    }

    public static IRuleBuilderOptions<T, string?> MustBeValidPassword<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$")
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.");
    }
}
