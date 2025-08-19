namespace Blog.FSharp.Contracts.V1.Responses.PostsResponses

open System
open System.Collections.Generic
open Blog.FSharp.Contracts.V1.Responses.UsersResponses
open Blog.FSharp.Contracts.V1.Responses.CommentsResponses
open Blog.FSharp.Contracts.V1.Responses.TagsResponses

/// <summary>
/// Post response.
/// </summary>
type PostResponse() =
    /// <summary>Gets or sets title.</summary>
    member val Title: string = "" with get, set

    /// <summary>Gets or sets description.</summary>
    member val Description: string = "" with get, set

    /// <summary>Gets or sets content.</summary>
    member val Content: string = "" with get, set

    /// <summary>Gets or sets seen.</summary>
    member val Seen: int = 0 with get, set

    /// <summary>Gets or sets likes.</summary>
    member val Likes: int = 0 with get, set

    /// <summary>Gets or sets dislikes.</summary>
    member val Dislikes: int = 0 with get, set

    /// <summary>Gets or sets image url.</summary>
    member val ImageUrl: string = "" with get, set

    /// <summary>Gets or sets author id.</summary>
    member val AuthorId: string = "" with get, set

    /// <summary>Gets or sets application user.</summary>
    member val Author: ApplicationUserResponse|null = null with get, set

    /// <summary>Gets or sets created at.</summary>
    member val CreatedAt: DateTime = DateTime.Now with get, set

    /// <summary>Gets or sets the comments.</summary>
    member val Comments: ICollection<CommentResponse> = List<CommentResponse>() :> ICollection<CommentResponse> with get, set

    /// <summary>Gets or sets the posts tags relations.</summary>
    member val PostsTagsRelations: ICollection<PostTagRelationsResponse> = List<PostTagRelationsResponse>() :> ICollection<PostTagRelationsResponse> with get, set

/// <summary>
/// Post tag relations response.
/// </summary>
and PostTagRelationsResponse() =
    /// <summary>Gets or sets the post identifier.</summary>
    member val PostId: int = 0 with get, set

    /// <summary>Gets or sets the post.</summary>
    member val Post: PostResponse|null = null with get, set

    /// <summary>Gets or sets the tag identifier.</summary>
    member val TagId: int = 0 with get, set

    /// <summary>Gets or sets the tag.</summary>
    member val Tag: TagResponse|null = null with get, set

