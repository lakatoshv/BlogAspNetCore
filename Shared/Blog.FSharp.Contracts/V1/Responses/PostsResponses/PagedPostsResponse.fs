namespace Blog.FSharp.Contracts.V1.Responses.PostsResponses

open System.Collections.Generic
open Blog.FSharp.Contracts.V1.Responses

/// <summary>
/// Paged posts response.
/// </summary>
type PagedPostsResponse() =

    /// <summary>
    /// Gets or sets posts.
    /// </summary>
    member val Posts : IList<PostViewResponse> = null with get, set

    /// <summary>
    /// Gets or sets display type.
    /// </summary>
    member val DisplayType : string = null with get, set

    /// <summary>
    /// Gets or sets page info.
    /// </summary>
    member val PageInfo : PageInfoResponse|null = null with get, set