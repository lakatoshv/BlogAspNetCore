using Blazor.Contracts.V1.Requests.MessagesRequests;
using FluentValidation;

namespace Blazor.Contracts.V1.Validators.MessagesValidators;

/// <summary>
/// Update message request validator.
/// </summary>
/// <seealso cref="AbstractValidator{UpdateMessageRequest}" />
public class UpdateMessageRequestValidator : AbstractValidator<UpdateMessageRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateMessageRequestValidator"/> class.
    /// </summary>
    public UpdateMessageRequestValidator()
    {
        RuleFor(x => x.Subject)
            .NotEmpty()
            .Matches("^[a-zA-Z0-9 ]*S");

        RuleFor(x => x.Body)
            .NotEmpty()
            .Matches("^[a-zA-Z0-9 ]*S");
    }
}