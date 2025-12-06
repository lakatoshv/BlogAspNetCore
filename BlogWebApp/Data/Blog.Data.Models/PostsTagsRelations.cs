// <copyright file="PostsTagsRelations.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Data.Models;

using Core;

/// <summary>
/// Posts Tags Many-to-Many Relations entity.
/// </summary>
/// <seealso cref="Entity" />
public class PostsTagsRelations : Entity
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
    public virtual Post Post { get; set; }

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
    public Tag Tag { get; set; }
}