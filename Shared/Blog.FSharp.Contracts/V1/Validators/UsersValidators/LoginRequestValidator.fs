namespace Blog.FSharp.Contracts.V1.Validators.UsersValidators

open FluentValidation
open Blog.FSharp.Contracts.V1.Requests.UsersRequests

/// <summary>
/// Validator for login request.
/// </summary>
type LoginRequestValidator() =
    inherit AbstractValidator<LoginRequest>()

    do
        // Validate Email: not empty, valid email format
        base.RuleFor(fun x -> x.Email)
            .NotEmpty()
            .EmailAddress() |> ignore

        // Validate Password: not empty, at least 6 chars, matches regex
        base.RuleFor(fun x -> x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .Matches("^[a-zA-Z0-9-_]*S") |> ignore