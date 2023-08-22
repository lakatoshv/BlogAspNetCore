// <copyright file="PostsSearchParametersDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Dtos.Posts;

/// <summary>
/// Posts search parameters dto.
/// </summary>
/// <seealso cref="SearchParametersDto" />
public class PostsSearchParametersDto : SearchParametersDto
{
    /// <summary>
    /// Gets or sets the tag.
    /// </summary>
    /// <value>
    /// The tag.
    /// </value>
    public string Tag { get; set; }
}