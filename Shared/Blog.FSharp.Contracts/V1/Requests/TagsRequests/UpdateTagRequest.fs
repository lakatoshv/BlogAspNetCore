namespace Blog.FSharp.Contracts.V1.Requests.TagsRequests

open Blog.FSharp.Contracts.V1.Requests.Interfaces

/// <summary>
/// Update tag request.
/// </summary>
type UpdateTagRequest() =
    interface IRequest

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    member val Title : string = null with get, set