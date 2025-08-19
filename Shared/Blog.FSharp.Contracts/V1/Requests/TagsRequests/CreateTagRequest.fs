namespace Blog.FSharp.Contracts.V1.Requests.TagsRequests

open Blog.FSharp.Contracts.V1.Requests.Interfaces

/// <summary>
/// Create tag request.
/// </summary>
type CreateTagRequest() =
    interface IRequest

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    member val Title : string = null with get, set