// <copyright file="IDeletableEntity.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Data.Core.Models.Interfaces;

using System;

/// <summary>
/// Deletable entity interface.
/// </summary>
public interface IDeletableEntity
{
    /// <summary>
    /// Gets or sets a value indicating whether is deleted.
    /// </summary>
    bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets deleted on.
    /// </summary>
    DateTime? DeletedOn { get; set; }
}