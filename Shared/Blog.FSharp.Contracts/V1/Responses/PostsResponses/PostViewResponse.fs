namespace Blog.FSharp.Contracts.V1.Responses.PostsResponses

open System
open System.Collections.Generic
open Blog.FSharp.Contracts.V1.Responses.UsersResponses
open Blog.FSharp.Contracts.V1.Responses.TagsResponses
open Blog.FSharp.Contracts.V1.Responses.CommentsResponses

/// <summary>
/// Post view response.
/// </summary>
type PostViewResponse() =
    /// <summary>
    /// Gets or sets id.
    /// </summary>
    member val Id: int = 0 with get, set

    /// <summary>
    /// Gets or sets title.
    /// </summary>
    member val Title: string = "" with get, set

    /// <summary>
    /// Gets or sets description.
    /// </summary>
    member val Description: string = "" with get, set

    /// <summary>
    /// Gets or sets content.
    /// </summary>
    member val Content: string = "" with get, set

    /// <summary>
    /// Gets or sets seen.
    /// </summary>
    member val Seen: int = 0 with get, set

    /// <summary>
    /// Gets or sets likes.
    /// </summary>
    member val Likes: int = 0 with get, set

    /// <summary>
    /// Gets or sets dislikes.
    /// </summary>
    member val Dislikes: int = 0 with get, set

    /// <summary>
    /// Gets or sets image url.
    /// </summary>
    member val ImageUrl: string = "" with get, set

    /// <summary>
    /// Gets or sets author id.
    /// </summary>
    member val AuthorId: string = "" with get, set

    /// <summary>
    /// Gets or sets application user.
    /// </summary>
    member val Author: ApplicationUserResponse = ApplicationUserResponse() with get, set

    /// <summary>
    /// Gets or sets created at.
    /// </summary>
    member val CreatedAt: DateTime = DateTime.Now with get, set

    /// <summary>
    /// Gets or sets the comments count.
    /// </summary>
    member val CommentsCount: int = 0 with get, set

    /// <summary>
    /// Gets or sets the comments.
    /// </summary>
    member val Comments: IList<CommentResponse> = List<CommentResponse>() :> IList<CommentResponse> with get, set

    /// <summary>
    /// Gets or sets the tags.
    /// </summary>
    member val Tags: IList<TagResponse> = List<TagResponse>() :> IList<TagResponse> with get, set