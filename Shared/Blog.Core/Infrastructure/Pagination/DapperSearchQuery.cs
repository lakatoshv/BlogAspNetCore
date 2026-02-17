// <copyright file="DapperSearchQuery.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Infrastructure.Pagination;

public class DapperSearchQuery
{
    /// <summary>
    /// Gets or sets the message
    /// </summary>
    public string? WhereClause { get; set; }

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    public object? Parameters { get; set; }

    /// <summary>
    /// Gets or sets order by.
    /// </summary>
    public string? OrderBy { get; set; }

    /// <summary>
    /// Gets or sets skip.
    /// </summary>
    public int Skip { get; set; }

    /// <summary>
    /// Gets or sets take.
    /// </summary>
    public int Take { get; set; }
}