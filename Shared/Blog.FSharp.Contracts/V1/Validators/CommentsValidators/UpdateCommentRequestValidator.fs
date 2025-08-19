namespace Blog.FSharp.Contracts.V1.Validators.CommentsValidators

open FluentValidation
open Blog.FSharp.Contracts.V1.Requests.CommentsRequests

/// <summary>
/// Validator for updating a comment request.
/// </summary>
type UpdateCommentRequestValidator() =
    inherit AbstractValidator<UpdateCommentRequest>()

    do
        // Ensure PostId is not -1
        base.RuleFor(fun x -> x.PostId)
            .NotEqual(-1) |> ignore

        // Ensure CommentBody is not empty and matches the regex
        base.RuleFor(fun x -> x.CommentBody)
            .NotEmpty()
            .Matches("^[a-zA-Z0-9 ]*S") |> ignore

        // Ensure UserId is not empty
        base.RuleFor(fun x -> x.UserId)
            .NotEmpty() |> ignore

