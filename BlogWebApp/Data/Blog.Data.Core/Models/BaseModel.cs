// <copyright file="BaseModel.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Data.Core.Models;

using Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Base model.
/// </summary>
/// <typeparam name="TKey">TKey.</typeparam>
public abstract class BaseModel<TKey>(TKey id)
    : IAuditInfo
{
    /// <summary>
    /// Gets or sets id.
    /// </summary>
    [Key]
    public TKey Id { get; } = id;

    /// <summary>
    /// Gets or sets created on.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets modified on.
    /// </summary>
    public DateTime? ModifiedOn { get; set; }
}