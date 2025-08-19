namespace Blog.FSharp.Contracts.V1.Responses.PostsResponses

open System.Collections.Generic
open Blog.FSharp.Contracts.V1.Responses.CommentsResponses
open Blog.FSharp.Contracts.V1.Responses.TagsResponses

/// <summary>
/// Post with paged comments response.
/// </summary>
type PostWithPagedCommentsResponse() =
    /// <summary>
    /// Gets or sets post.
    /// </summary>
    member val Post: PostViewResponse = PostViewResponse() with get, set

    /// <summary>
    /// Gets or sets the comments.
    /// </summary>
    member val Comments: PagedCommentsResponse = PagedCommentsResponse() with get, set

    /// <summary>
    /// Gets or sets the tags.
    /// </summary>
    member val Tags: IList<TagResponse> = List<TagResponse>() :> IList<TagResponse> with get, set

