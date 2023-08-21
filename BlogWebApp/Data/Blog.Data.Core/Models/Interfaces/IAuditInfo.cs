// <copyright file="IAuditInfo.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Data.Core.Models.Interfaces;

using System;

/// <summary>
/// Audit info interface.
/// </summary>
public interface IAuditInfo
{
    /// <summary>
    /// Gets or sets created on.
    /// </summary>
    DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets modified on.
    /// </summary>
    DateTime? ModifiedOn { get; set; }
}