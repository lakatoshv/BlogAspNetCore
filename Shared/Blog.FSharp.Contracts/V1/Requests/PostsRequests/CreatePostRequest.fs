namespace Blog.FSharp.Contracts.V1.Requests.PostsRequests

open System
open System.Collections.Generic
open System.ComponentModel.DataAnnotations
open Blog.FSharp.Contracts.V1.Requests.Interfaces
open Blog.FSharp.Contracts.V1.Requests.TagsRequests

/// <summary>
/// Create post request.
/// </summary>
type CreatePostRequest() =
    interface IRequest

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