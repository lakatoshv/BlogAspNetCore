// <copyright file="SearchQuery.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Infrastructure.Pagination;

using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

/// <summary>
/// Search query.
/// </summary>
/// <typeparam name="TEntity">TEntity.</typeparam>
public class SearchQuery<TEntity>
{
    /// <summary>
    /// Gets or sets sort criteria.
    /// </summary>
    public List<ISortCriteria<TEntity>> SortCriterias
    {
        get;
        protected set;
    }

    /// <summary>
    /// Gets or sets include properties.
    /// </summary>
    public string IncludeProperties { get; set; }

    /// <summary>
    /// Gets or sets skip.
    /// </summary>
    public int Skip { get; set; }

    /// <summary>
    /// Gets or sets take.
    /// </summary>
    public int Take { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchQuery{TEntity}"/> class.
    /// </summary>
    public SearchQuery()
    {
        this.Filters = [];
        this.SortCriterias = [];
    }

    /// <summary>
    /// Gets or sets filters.
    /// </summary>
    public List<Expression<Func<TEntity, bool>>> Filters { get; protected set; }

    /// <summary>
    /// Add filter.
    /// </summary>
    /// <param name="filter">filter.</param>
    public void AddFilter(Expression<Func<TEntity, bool>> filter)
    {
        this.Filters.Add(filter);
    }

    /// <summary>
    /// Add sort criteria params.
    /// </summary>
    /// <param name="sortCriteria">sortCriteria.</param>
    public void AddSortCriteria(ISortCriteria<TEntity> sortCriteria)
    {
        this.SortCriterias.Add(sortCriteria);
    }
}