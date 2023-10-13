using Blazor.Contracts.V1.Requests.TagsRequests;
using FluentValidation;

namespace Blazor.Contracts.V1.Validators.TagsValidators;

/// <summary>
/// Update tag request validator.
/// </summary>
/// <seealso cref="AbstractValidator{UpdateTagRequest}" />
public class UpdateTagRequestValidator : AbstractValidator<UpdateTagRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateTagRequestValidator"/> class.
    /// </summary>
    public UpdateTagRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(2)
            .MinimumLength(64)
            .Matches("^[a-zA-Z0-9 ]*S");
    }
}