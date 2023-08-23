// <copyright file="TableFilter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.TableFilters;

using System.Collections.Generic;
using System.Linq;
using Enums;

/// <summary>
/// Table filter.
/// </summary>
public class TableFilter
{
    /// <summary>
    /// Gets or sets start.
    /// </summary>
    public int Start { get; set; }

    /// <summary>
    /// Gets or sets length.
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    /// Gets or sets search.
    /// </summary>
    public TableSearchModel Search { get; set; }

    /// <summary>
    /// Gets or sets order.
    /// </summary>
    public IEnumerable<TableSortingModel> Order { get; set; }

    /// <summary>
    /// Gets or sets columns.
    /// </summary>
    public List<TableColumn> Columns { get; set; }

    /// <summary>
    /// Gets page count.
    /// </summary>
    public int PageCount => (this.Start / this.Length) + 1;

    /// <summary>
    /// Gets page size.
    /// </summary>
    public int PageSize => this.Length;

    /// <summary>
    /// Gets order type.
    /// </summary>
    public OrderType OrderType => this.Order.FirstOrDefault()?.Dir == "asc"
        ? OrderType.Ascending
        : OrderType.Descending;

    /// <summary>
    /// Gets column name.
    /// </summary>
    public string ColumnName => this.Order.Any() ? this.Columns[this.Order.First().Column].Data : string.Empty;
}