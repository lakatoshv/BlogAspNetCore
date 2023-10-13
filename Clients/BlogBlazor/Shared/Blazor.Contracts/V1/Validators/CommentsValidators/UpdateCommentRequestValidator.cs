using Blazor.Contracts.V1.Requests.CommentsRequests;
using FluentValidation;

namespace Blazor.Contracts.V1.Validators.CommentsValidators;

/// <summary>
/// Update comment request validator.
/// </summary>
/// <seealso cref="AbstractValidator{UpdateCommentRequest}" />
public class UpdateCommentRequestValidator : AbstractValidator<UpdateCommentRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCommentRequestValidator"/> class.
    /// </summary>
    public UpdateCommentRequestValidator()
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