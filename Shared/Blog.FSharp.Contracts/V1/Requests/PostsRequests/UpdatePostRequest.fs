namespace Blog.FSharp.Contracts.V1.Requests.PostsRequests

open System
open System.Collections.Generic
open System.ComponentModel.DataAnnotations
open Blog.FSharp.Contracts.V1.Requests.Interfaces
open Blog.FSharp.Contracts.V1.Requests.TagsRequests


/// <summary>
/// Update post request.
/// </summary>
type UpdatePostRequest() =
    interface IRequest

    /// <summary>
    /// Gets or sets id.
    /// </summary>
    member val Id : int = 0 with get, set

    /// <summary>
    /// Gets or sets title.
    /// </summary>
    [<Required>]
    member val Title : string = null with get, set

    /// <summary>
    /// Gets or sets description.
    /// </summary>
    [<Required>]
    member val Description : string = null with get, set

    /// <summary>
    /// Gets or sets content.
    /// </summary>
    [<Required>]
    member val Content : string = null with get, set

    /// <summary>
    /// Gets or sets seen.
    /// </summary>
    member val Seen : int = 0 with get, set

    /// <summary>
    /// Gets or sets likes.
    /// </summary>
    member val Likes : int = 0 with get, set

    /// <summary>
    /// Gets or sets dislikes.
    /// </summary>
    member val Dislikes : int = 0 with get, set

    /// <summary>
    /// Gets or sets image url.
    /// </summary>
    member val ImageUrl : string = null with get, set

    /// <summary>
    /// Gets or sets author id.
    /// </summary>
    member val AuthorId : string = null with get, set

    /// <summary>
    /// Gets or sets created at.
    /// </summary>
    [<DataType(DataType.DateTime)>]
    member val CreatedAt : DateTime = DateTime.MinValue with get, set

    /// <summary>
    /// Gets or sets the tags.
    /// </summary>
    member val Tags : IList<TagRequest> = null with get, set

