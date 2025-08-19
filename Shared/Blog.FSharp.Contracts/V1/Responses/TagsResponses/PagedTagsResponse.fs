namespace Blog.FSharp.Contracts.V1.Responses.TagsResponses

open System.Collections.Generic
open Blog.FSharp.Contracts.V1.Responses

/// <summary>
/// Paged tags response.
/// </summary>
type PagedTagsResponse() =
    /// <summary>
    /// Gets or sets the tags.
    /// </summary>
    member val Tags: IList<TagResponse> = List<TagResponse>() :> IList<TagResponse> with get, set

    /// <summary>
    /// Gets or sets display type.
    /// </summary>
    member val DisplayType: string = "" with get, set

    /// <summary>
    /// Gets or sets page info.
    /// </summary>
    member val PageInfo: PageInfoResponse = PageInfoResponse() with get, set

