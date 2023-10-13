namespace Blazor.Contracts.V1.Responses.PostsResponses;

using TagsResponses;

/// <summary>
/// Post tag relations response.
/// </summary>
public class PostTagRelationsResponse
{
    /// <summary>
    /// Gets or sets the post identifier.
    /// </summary>
    /// <value>
    /// The post identifier.
    /// </value>
    public int PostId { get; set; }

    /// <summary>
    /// Gets or sets the post.
    /// </summary>
    /// <value>
    /// The post.
    /// </value>
    public virtual PostResponse Post { get; set; }

    /// <summary>
    /// Gets or sets the tag identifier.
    /// </summary>
    /// <value>
    /// The tag identifier.
    /// </value>
    public int TagId { get; set; }

    /// <summary>
    /// Gets or sets the tag.
    /// </summary>
    /// <value>
    /// The tag.
    /// </value>
    public TagResponse Tag { get; set; }
}