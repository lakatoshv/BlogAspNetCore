namespace Blog.FSharp.Contracts.V1.Validators.CommentsValidators

open FluentValidation
open Blog.FSharp.Contracts.V1.Requests.CommentsRequests

/// <summary>
/// Create comment request validator.
/// </summary>
type CreateCommentRequestValidator() =
    inherit AbstractValidator<CreateCommentRequest>()

    do
        // PostId should not be -1
        base.RuleFor(fun x -> x.PostId)
            .NotEqual(-1) |> ignore

        // CommentBody is required and shold match to Regex
        base.RuleFor(fun x -> x.CommentBody)
            .NotEmpty()
            .Matches("^[a-zA-Z0-9 ]*S") |> ignore

        // UserId is required
        base.RuleFor(fun x -> x.UserId)
            .NotEmpty() |> ignore