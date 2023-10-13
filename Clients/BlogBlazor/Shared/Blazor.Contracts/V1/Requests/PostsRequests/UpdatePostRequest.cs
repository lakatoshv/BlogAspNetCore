namespace Blazor.Contracts.V1.Requests.PostsRequests;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TagsRequests;
using Interfaces;

/// <summary>
/// Update post request.
/// </summary>
public class UpdatePostRequest : IRequest
{
    /// <summary>
    /// Gets or sets id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets title.
    /// </summary>
    [Required]
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets description.
    /// </summary>
    [Required]
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets content.
    /// </summary>
    [Required]
    public string Content { get; set; }

    /// <summary>
    /// Gets or sets seen.
    /// </summary>
    public int Seen { get; set; }

    /// <summary>
    /// Gets or sets likes.
    /// </summary>
    public int Likes { get; set; }

    /// <summary>
    /// Gets or sets dislikes.
    /// </summary>
    public int Dislikes { get; set; }

    /// <summary>
    /// Gets or sets image url.
    /// </summary>
    public string ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets author id.
    /// </summary>
    public string AuthorId { get; set; }

    /// <summary>
    /// Gets or sets created at.
    /// </summary>
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the tags.
    /// </summary>
    /// <value>
    /// The tags.
    /// </value>
    public IList<TagRequest> Tags { get; set; }
}