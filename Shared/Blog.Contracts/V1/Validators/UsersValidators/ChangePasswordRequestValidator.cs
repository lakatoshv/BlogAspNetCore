using Blog.Contracts.V1.Requests.UsersRequests;
using FluentValidation;

namespace Blog.Contracts.V1.Validators.UsersValidators
{
    /// <summary>
    /// Change password request validator.
    /// </summary>
    /// <seealso cref="AbstractValidator{ChangePasswordRequest}" />
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangePasswordRequestValidator"/> class.
        /// </summary>
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty()
                .MinimumLength(6)
                .Matches("^[a-zA-Z0-9-_]*S");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(6)
                .Matches("^[a-zA-Z0-9-_]*S");
        }
    }
}
