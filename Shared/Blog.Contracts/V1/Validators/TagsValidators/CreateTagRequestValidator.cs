using Blog.Contracts.V1.Requests.TagsRequests;
using FluentValidation;

namespace Blog.Contracts.V1.Validators.TagsValidators
{
    /// <summary>
    /// Create tag request validator.
    /// </summary>
    /// <seealso cref="AbstractValidator{CreateTagRequest}" />
    public class CreateTagRequestValidator : AbstractValidator<CreateTagRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTagRequestValidator"/> class.
        /// </summary>
        public CreateTagRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MinimumLength(2)
                .MinimumLength(64)
                .Matches("^[a-zA-Z0-9 ]*S");
        }
    }
}
