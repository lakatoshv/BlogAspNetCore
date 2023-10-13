using Blazor.Contracts.V1.Requests.MessagesRequests;
using FluentValidation;

namespace Blazor.Contracts.V1.Validators.MessagesValidators;

/// <summary>
/// Create message request validator.
/// </summary>
/// <seealso cref="AbstractValidator{CreateMessageRequest}" />
public class CreateMessageRequestValidator : AbstractValidator<CreateMessageRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateMessageRequestValidator"/> class.
    /// </summary>
    public CreateMessageRequestValidator()
    {
        RuleFor(x => x.Subject)
            .NotEmpty()
            .Matches("^[a-zA-Z0-9 ]*S");

        RuleFor(x => x.Body)
            .NotEmpty()
            .Matches("^[a-zA-Z0-9 ]*S");
    }
}