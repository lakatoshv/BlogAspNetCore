// <copyright file="CommentsViewDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Dtos.Posts;

using System.Collections.Generic;
using Blog.Core.Helpers;
using Data.Models;

/// <summary>
/// Comments view dto.
/// </summary>
public class CommentsViewDto
{
    /// <summary>
    /// Gets or sets the comments.
    /// </summary>
    /// <value>
    /// The comments.
    /// </value>
    public IList<Comment> Comments { get; set; }

    /// <summary>
    /// Gets or sets the page information.
    /// </summary>
    /// <value>
    /// The page information.
    /// </value>
    public PageInfo PageInfo { get; set; }
}