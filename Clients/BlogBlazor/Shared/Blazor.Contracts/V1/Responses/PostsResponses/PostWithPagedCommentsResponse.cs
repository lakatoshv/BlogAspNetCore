namespace Blazor.Contracts.V1.Responses.PostsResponses;

using System.Collections.Generic;
using CommentsResponses;
using TagsResponses;

/// <summary>
/// Post with paged comments response.
/// </summary>
public class PostWithPagedCommentsResponse
{
    /// <summary>
    /// Gets or sets post.
    /// </summary>
    public PostViewResponse Post { get; set; }

    /// <summary>
    /// Gets or sets the comments.
    /// </summary>
    /// <value>
    /// The comments.
    /// </value>
    public PagedCommentsResponse Comments { get; set; }

    // public Comment Comment { get; set; }
    // public int CommentsCount { get; set; }
    // public Profile Profile { get; set; }

    /// <summary>
    /// Gets or sets the tags.
    /// </summary>
    /// <value>
    /// The tags.
    /// </value>
    public IList<TagResponse> Tags { get; set; }
}