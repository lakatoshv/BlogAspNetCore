// <copyright file="StreamPageFilter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Infrastructure.PageFilter;

/// <summary>
/// Stream page filter.
/// </summary>
public class StreamPageFilter
{
    /// <summary>
    /// Gets or sets previous value.
    /// </summary>
    public int PreviousValue { get; set; }

    /// <summary>
    /// Gets or sets length.
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    /// Gets or sets page count.
    /// </summary>
    public int PageCount { get; set; }
}