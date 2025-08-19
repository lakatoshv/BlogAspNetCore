namespace Blog.FSharp.Contracts.V1.Validators.MessagesValidators

open FluentValidation
open Blog.FSharp.Contracts.V1.Requests.MessagesRequests

/// <summary>
/// Validator for updating a message request.
/// </summary>
type UpdateMessageRequestValidator() =
    inherit AbstractValidator<UpdateMessageRequest>()

    do
        // Ensure Subject is not empty and matches the regex
        base.RuleFor(fun x -> x.Subject)
            .NotEmpty()
            .Matches("^[a-zA-Z0-9 ]*S") |> ignore

        // Ensure Body is not empty and matches the regex
        base.RuleFor(fun x -> x.Body)
            .NotEmpty()
            .Matches("^[a-zA-Z0-9 ]*S") |> ignore
