using Blazor.Contracts.V1.Requests.TagsRequests;
using FluentValidation;

namespace Blazor.Contracts.V1.Validators.TagsValidators
{
    /// <summary>
    /// Tag request validator.
    /// </summary>
    /// <seealso cref="AbstractValidator{TagRequest}" />
    public class TagRequestValidator : AbstractValidator<TagRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagRequestValidator"/> class.
        /// </summary>
        public TagRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MinimumLength(2)
                .MinimumLength(64)
                .Matches("^[a-zA-Z0-9 ]*S");
        }
    }
}
