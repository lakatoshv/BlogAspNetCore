namespace Blog.FSharp.Contracts.V1.Validators.UsersValidators

open FluentValidation
open Blog.FSharp.Contracts.V1.Requests.UsersRequests

/// <summary>
/// Validator for change password request.
/// </summary>
type ChangePasswordRequestValidator() =
    inherit AbstractValidator<ChangePasswordRequest>()

    do
        // Validate OldPassword: not empty, at least 6 chars, matches regex
        base.RuleFor(fun x -> x.OldPassword)
            .NotEmpty()
            .MinimumLength(6)
            .Matches("^[a-zA-Z0-9-_]*S") |> ignore

        // Validate NewPassword: not empty, at least 6 chars, matches regex
        base.RuleFor(fun x -> x.NewPassword)
            .NotEmpty()
            .MinimumLength(6)
            .Matches("^[a-zA-Z0-9-_]*S") |> ignore