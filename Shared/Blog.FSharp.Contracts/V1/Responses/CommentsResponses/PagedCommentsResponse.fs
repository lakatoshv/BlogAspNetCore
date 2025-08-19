namespace Blog.FSharp.Contracts.V1.Responses.CommentsResponses

open System.Collections.Generic
open Blog.FSharp.Contracts.V1.Responses

/// <summary>
/// Paged comments response.
/// </summary>
type PagedCommentsResponse() =
    /// <summary>
    /// Gets or sets the comments.
    /// </summary>
    member val Comments: IList<CommentResponse> = ResizeArray<CommentResponse>() :> IList<CommentResponse> with get, set

    /// <summary>
    /// Gets or sets the page information.
    /// </summary>
    member val PageInfo: PageInfoResponse = PageInfoResponse() with get, set

