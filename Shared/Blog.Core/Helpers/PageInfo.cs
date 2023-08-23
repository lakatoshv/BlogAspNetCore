// <copyright file="PageInfo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Helpers;

using System;

/// <summary>
/// Page Info.
/// </summary>
public class PageInfo
{
    /// <summary>
    /// Gets or sets current page number.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Gets or sets page size.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets total items count.
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Gets total pages count.
    /// </summary>
    public int TotalPages
    {
        get { return (int)Math.Ceiling((decimal)this.TotalItems / this.PageSize); }
    }
}