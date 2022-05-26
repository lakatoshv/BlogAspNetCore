using Blazor.Contracts.V1.Requests.CommentsRequests;
using FluentValidation;

namespace Blazor.Contracts.V1.Validators.CommentsValidators
{
    /// <summary>
    /// Create comment request validator.
    /// </summary>
    /// <seealso cref="AbstractValidator{CreateCommentRequest}" />
    public class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommentRequestValidator"/> class.
        /// </summary>
        public CreateCommentRequestValidator()
        {
            RuleFor(x => x.PostId)
                .NotEqual(-1);

            RuleFor(x => x.CommentBody)
                .NotEmpty()
                .Matches("^[a-zA-Z0-9 ]*S");

            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
