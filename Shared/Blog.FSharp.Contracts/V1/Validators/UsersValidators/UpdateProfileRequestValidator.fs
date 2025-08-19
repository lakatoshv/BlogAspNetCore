namespace Blog.FSharp.Contracts.V1.Validators.UsersValidators

open FluentValidation
open System
open System.Linq.Expressions
open Blog.FSharp.Contracts.V1.Requests.UsersRequests

/// <summary>
/// Update profile request validator.
/// </summary>
type UpdateProfileRequestValidator() =
    inherit AbstractValidator<UpdateProfileRequest>() 

    do
        // Email must not be empty and must be in a valid email format
        base.RuleFor(fun x -> x.Email)
            .NotEmpty()
            .EmailAddress()
        |> ignore

        // FirstName must not be empty, must contain only letters,
        // and must be between 2 and 64 characters long
        base.RuleFor(fun x -> x.FirstName)
            .NotEmpty()
            .Matches("^[a-zA-Z]*$")
            .MinimumLength(2)
            .MaximumLength(64)
        |> ignore

        // LastName must not be empty, must contain only letters,
        // and must be between 2 and 64 characters long
        base.RuleFor(fun x -> x.LastName)
            .NotEmpty()
            .Matches("^[a-zA-Z]*$")
            .MinimumLength(2)
            .MaximumLength(64)
        |> ignore

        // PhoneNumber must match a valid phone number pattern (+ optional, 10-15 digits)
        base.RuleFor(fun x -> x.PhoneNumber)
            .Matches(@"^\+?\d{10,15}$")
        |> ignore