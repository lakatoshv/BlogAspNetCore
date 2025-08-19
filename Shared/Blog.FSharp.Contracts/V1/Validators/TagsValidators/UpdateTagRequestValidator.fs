namespace Blog.FSharp.Contracts.V1.Validators.TagsValidators

open FluentValidation
open Blog.FSharp.Contracts.V1.Requests.TagsRequests

/// <summary>
/// Validator for updating a tag request.
/// </summary>
type UpdateTagRequestValidator() =
    inherit AbstractValidator<UpdateTagRequest>()

    do
        // Ensure Title is not empty, has a length between 2 and 64, and matches the regex
        base.RuleFor(fun x -> x.Title)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(64)
            .Matches("^[a-zA-Z0-9 ]*S") |> ignore