namespace Blog.FSharp.Contracts.V1.Validators.MessagesValidators

open FluentValidation
open Blog.FSharp.Contracts.V1.Requests.MessagesRequests

/// <summary>
/// Validator for creating a message request.
/// </summary>
type CreateMessageRequestValidator() =
    inherit AbstractValidator<CreateMessageRequest>()

    do
        // Ensure Subject is not empty and matches the regex
        base.RuleFor(fun x -> x.Subject)
            .NotEmpty()
            .Matches("^[a-zA-Z0-9 ]*S") |> ignore

        // Ensure Body is not empty and matches the regex
        base.RuleFor(fun x -> x.Body)
            .NotEmpty()
            .Matches("^[a-zA-Z0-9 ]*S") |> ignore

