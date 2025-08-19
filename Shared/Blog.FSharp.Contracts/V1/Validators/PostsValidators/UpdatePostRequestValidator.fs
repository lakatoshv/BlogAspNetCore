namespace Blog.FSharp.Contracts.V1.Validators.PostsValidators

open FluentValidation
open Blog.FSharp.Contracts.V1.Requests.PostsRequests

/// <summary>
/// Validator for updating a post request.
/// </summary>
type UpdatePostRequestValidator() =
    inherit AbstractValidator<UpdatePostRequest>()

    do
        // Ensure Title is not empty and matches the regex
        base.RuleFor(fun x -> x.Title)
            .NotEmpty()
            .Matches("^[a-zA-Z0-9 ]*S") |> ignore

        // Ensure Description is not empty
        base.RuleFor(fun x -> x.Description)
            .NotEmpty() |> ignore

        // Ensure Content is not empty
        base.RuleFor(fun x -> x.Content)
            .NotEmpty() |> ignore

        // Ensure AuthorId is not empty
        base.RuleFor(fun x -> x.AuthorId)
            .NotEmpty() |> ignore