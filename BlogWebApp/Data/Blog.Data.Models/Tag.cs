// <copyright file="Tag.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Data.Models;

using System.Collections.Generic;
using Core;

/// <summary>
/// Tag entity.
/// </summary>
/// <seealso cref="Entity" />
public class Tag : Entity
{
    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the posts tags relations.
    /// </summary>
    /// <value>
    /// The posts tags relations.
    /// </value>
    public ICollection<PostsTagsRelations> PostsTagsRelations { get; set; }
}