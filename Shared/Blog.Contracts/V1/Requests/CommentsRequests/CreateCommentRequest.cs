namespace Blog.Contracts.V1.Requests.CommentsRequests;

using System.ComponentModel.DataAnnotations;
using Interfaces;

/// <summary>
/// Create comment request.
/// </summary>
public class CreateCommentRequest : IRequest
{
    /// <summary>
    /// Gets or sets the post identifier.
    /// </summary>
    /// <value>
    /// The post identifier.
    /// </value>
    [Required]
    public int PostId { get; set; }

    /// <summary>
    /// Gets or sets the comment body.
    /// </summary>
    /// <value>
    /// The comment body.
    /// </value>
    [Required]
    public string CommentBody { get; set; }

    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    /// <value>
    /// The user identifier.
    /// </value>
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    /// <value>
    /// The email.
    /// </value>
    public string Email { get; set; }
}