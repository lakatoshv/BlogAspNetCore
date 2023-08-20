namespace Blog.Contracts.V1.Responses.CommentsResponses;

using System.Collections.Generic;

/// <summary>
/// Paged comments response.
/// </summary>
public class PagedCommentsResponse
{
    /// <summary>
    /// Gets or sets the comments.
    /// </summary>
    /// <value>
    /// The comments.
    /// </value>
    public IList<CommentResponse> Comments { get; set; }

    /// <summary>
    /// Gets or sets the page information.
    /// </summary>
    /// <value>
    /// The page information.
    /// </value>
    public PageInfoResponse PageInfo { get; set; }
}