namespace Blog.FSharp.Contracts.V1.Responses.CommentsResponses

open System
open System.ComponentModel.DataAnnotations
open Blog.FSharp.Contracts.V1.Responses.UsersResponses

/// <summary>
/// Comment response.
/// </summary>
type CommentResponse() =
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    member val Id: int option = None with get, set

    /// <summary>
    /// Gets or sets the post identifier.
    /// </summary>
    [<Required>]
    member val PostId: int = 0 with get, set

    /// <summary>
    /// Gets or sets the comment body.
    /// </summary>
    [<Required>]
    member val CommentBody: string = String.Empty with get, set

    /// <summary>
    /// Gets or sets the created at.
    /// </summary>
    [<DataType(DataType.DateTime)>]
    member val CreatedAt: DateTime = DateTime.MinValue with get, set

    /// <summary>
    /// Gets or sets the likes.
    /// </summary>
    member val Likes: int = 0 with get, set

    /// <summary>
    /// Gets or sets the dislikes.
    /// </summary>
    member val Dislikes: int = 0 with get, set

    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    member val UserId: string = String.Empty with get, set

    /// <summary>
    /// Gets or sets the user.
    /// </summary>
    member val User: ApplicationUserResponse option = None with get, set

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    member val Name: string = String.Empty with get, set

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    member val Email: string = String.Empty with get, set

