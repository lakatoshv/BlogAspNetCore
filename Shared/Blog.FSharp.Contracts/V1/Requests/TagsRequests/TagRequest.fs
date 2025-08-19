namespace Blog.FSharp.Contracts.V1.Requests.TagsRequests

open Blog.FSharp.Contracts.V1.Requests.Interfaces

/// <summary>
/// Tag request.
/// </summary>
type TagRequest() =
    interface IRequest

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    member val Title : string = null with get, set