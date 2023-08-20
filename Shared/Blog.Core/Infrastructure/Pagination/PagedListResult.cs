// <copyright file="PagedListResult.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Infrastructure.Pagination;

using System.Collections.Generic;

/// <summary>
/// Paged list.
/// </summary>
/// <typeparam name="TEntity">TEntity.</typeparam>
public class PagedListResult<TEntity>
{
    /// <summary>
    /// Gets or sets a value indicating whether has next value.
    /// </summary>
    public bool HasNext { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether has previous value.
    /// </summary>
    public bool HasPrevious { get; set; }

    /// <summary>
    /// Gets or sets count.
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Gets or sets entities.
    /// </summary>
    public IEnumerable<TEntity> Entities { get; set; }
}