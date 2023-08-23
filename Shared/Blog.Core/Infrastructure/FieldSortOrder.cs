// <copyright file="FieldSortOrder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Infrastructure;

using System.Linq;
using Enums;
using Blog.Core.Infrastructure.Pagination.Interfaces;

/// <summary>
/// Field sort order.
/// </summary>
/// <typeparam name="T">Type.</typeparam>
public class FieldSortOrder<T> : ISortCriteria<T>
    where T : class
{
    /// <summary>
    /// Gets or sets name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets direction.
    /// </summary>
    public OrderType Direction { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FieldSortOrder{T}"/> class.
    /// </summary>
    public FieldSortOrder()
    {
        this.Direction = OrderType.Ascending;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FieldSortOrder{T}"/> class.
    /// </summary>
    /// <param name="name">name.</param>
    /// <param name="direction">direction.</param>
    public FieldSortOrder(string name, OrderType direction)
    {
        this.Name = name;
        this.Direction = direction;
    }

    /// <inheritdoc cref="ISortCriteria{T}" />
    public IOrderedQueryable<T> ApplyOrdering(IQueryable<T> qry, bool useThenBy)
    {
        IOrderedQueryable<T> result;
        var descending = this.Direction == OrderType.Descending;
        result = !useThenBy ? qry.OrderBy(this.Name, descending) : qry.ThenBy(this.Name, descending);
        return result;
    }
}