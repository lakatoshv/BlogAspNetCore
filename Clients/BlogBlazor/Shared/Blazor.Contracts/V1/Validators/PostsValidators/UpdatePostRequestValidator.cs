using Blazor.Contracts.V1.Requests.PostsRequests;
using FluentValidation;

namespace Blazor.Contracts.V1.Validators.PostsValidators;

/// <summary>
/// Update post request validator.
/// </summary>
/// <seealso cref="AbstractValidator{UpdatePostRequest}" />
class UpdatePostRequestValidator : AbstractValidator<UpdatePostRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePostRequestValidator"/> class.
    /// </summary>
    public UpdatePostRequestValidator()
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