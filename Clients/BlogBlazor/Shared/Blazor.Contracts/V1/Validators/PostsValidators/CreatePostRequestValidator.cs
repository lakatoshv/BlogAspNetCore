using Blazor.Contracts.V1.Requests.PostsRequests;
using FluentValidation;

namespace Blazor.Contracts.V1.Validators.PostsValidators;

/// <summary>
/// Create post request validator.
/// </summary>
/// <seealso cref="AbstractValidator{UpdatePostRequest}" />
class CreatePostRequestValidator : AbstractValidator<CreatePostRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePostRequestValidator"/> class.
    /// </summary>
    public CreatePostRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .Matches("^[a-zA-Z0-9 ]*S");

        RuleFor(x => x.Description)
            .NotEmpty();

        RuleFor(x => x.Content)
            .NotEmpty();

        RuleFor(x => x.AuthorId)
            .NotEmpty();
    }
}