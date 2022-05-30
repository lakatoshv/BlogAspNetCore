using Blazor.Contracts.V1.Requests.UsersRequests;
using FluentValidation;

namespace Blazor.Contracts.V1.Validators.UsersValidators
{
    /// <summary>
    /// Login request validator.
    /// </summary>
    /// <seealso cref="AbstractValidator{LoginRequest}" />
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTagRequestValidator"/> class.
        /// </summary>
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .Matches("^[a-zA-Z0-9-_]*S");
        }
    }
}
