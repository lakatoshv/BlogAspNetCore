namespace Blog.FSharp.Contracts.V1.Requests.CommentsRequests

open System.ComponentModel.DataAnnotations
open Blog.FSharp.Contracts.V1.Requests.Interfaces

/// <summary>
/// Create comment request.
/// </summary>
type CreateCommentRequest() =
    interface IRequest

    /// <summary>
    /// Gets or sets the post identifier.
    /// </summary>
    [<Required>]
    member val PostId: int = 0 with get, set

    /// <summary>
    /// Gets or sets the comment body.
    /// </summary>
    [<Required>]
    member val CommentBody: string = "" with get, set

    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    member val UserId: string = "" with get, set

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    member val Name: string = "" with get, set

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    member val Email: string = "" with get, set