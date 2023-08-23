// <copyright file="TableSortingModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.TableFilters;

/// <summary>
/// Table sorting model.
/// </summary>
public class TableSortingModel
{
    /// <summary>
    /// Gets or sets column.
    /// </summary>
    public int Column { get; set; }

    /// <summary>
    /// Gets or sets dir.
    /// </summary>
    public string Dir { get; set; }
}