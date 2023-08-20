using Blog.Contracts.V1.Requests.UsersRequests;
using FluentValidation;

namespace Blog.Contracts.V1.Validators.UsersValidators;

/// <summary>
/// Change password request validator.
/// </summary>
/// <seealso cref="AbstractValidator{RegistrationRequest}" />
public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Matches("^[a-zA-Z]*S")
            .MinimumLength(2)
            .MaximumLength(64);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Matches("^[a-zA-Z]*S")
            .MinimumLength(2)
            .MaximumLength(64);

        RuleFor(x => x.PhoneNumber)
            .Matches(@"(?:(?:(\s*\(?([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*)|([2-9]1[02-9]|[2‌​-9][02-8]1|[2-9][02-8][02-9]))\)?\s*(?:[.-]\s*)?)([2-9]1[02-9]|[2-9][02-9]1|[2-9]‌​[02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})");
    }
}