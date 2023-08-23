// <copyright file="Setting.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Data.Models;

using Core;

/// <summary>
/// Setting.
/// </summary>

public class Setting : Entity
{
    /// <summary>
    /// Gets or sets name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets value.
    /// </summary>
    public string Value { get; set; }
}