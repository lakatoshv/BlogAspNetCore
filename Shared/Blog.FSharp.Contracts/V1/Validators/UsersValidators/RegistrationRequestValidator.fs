namespace Blog.FSharp.Contracts.V1.Validators.UsersValidators

open FluentValidation
open System
open System.Linq.Expressions
open Blog.FSharp.Contracts.V1.Requests.UsersRequests

/// <summary>
/// Validator for registration request.
/// </summary>
type RegistrationRequestValidator() =
    inherit AbstractValidator<RegistrationRequest>()

    // робимо Expression для Password
    static let passwordExpr : Expression<Func<RegistrationRequest, string>> =
        Expression.Lambda<Func<RegistrationRequest, string>>(
            Expression.Property(Expression.Parameter(typeof<RegistrationRequest>, "x"), "Password"),
            [| Expression.Parameter(typeof<RegistrationRequest>, "x") |])

    do
        // Validate Email: must not be empty, must have a valid email format
        base.RuleFor(fun x -> x.Email)
            .NotEmpty()
            .EmailAddress() |> ignore

        // Validate Password: must not be empty, must match regex, must be at least 6 characters long
        base.RuleFor(fun x -> x.Password)
            .NotEmpty()
            .Matches("^[a-zA-Z0-9-_]*S")
            .MinimumLength(6) |> ignore

        // Validate ConfirmPassword: must not be empty, must match regex, must be at least 6 characters long,
        // and must be equal to Password
        base.RuleFor(fun x -> x.ConfirmPassword)
            .NotEmpty()
            .Matches("^[a-zA-Z0-9-_]*S")
            .MinimumLength(6)
            //TODO: Make it workable
            //.Equal(passwordExpr)
            |> ignore

        // Validate FirstName: must not be empty, must contain only alphabetic characters, 
        // must be between 2 and 64 characters
        base.RuleFor(fun x -> x.FirstName)
            .NotEmpty()
            .Matches("^[a-zA-Z]*S")
            .MinimumLength(2)
            .MaximumLength(64) |> ignore

        // Validate LastName: must not be empty, must contain only alphabetic characters, 
        // must be between 2 and 64 characters
        base.RuleFor(fun x -> x.LastName)
            .NotEmpty()
            .Matches("^[a-zA-Z]*S")
            .MinimumLength(2)
            .MaximumLength(64) |> ignore

        // Validate PhoneNumber: must match a complex regex for phone number format
        base.RuleFor(fun x -> x.PhoneNumber)
            .Matches(@"(?:(?:(\s*\(?([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*)|([2-9]1[02-9]|[2‌​-9][02-8]1|[2-9][02-8][02-9]))\)?\s*(?:[.-]\s*)?)([2-9]1[02-9]|[2-9][02-9]1|[2-9]‌​[02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})") |> ignore
