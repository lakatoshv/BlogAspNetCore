// <copyright file="IControllerContext.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Blog.Data.Models;

namespace Blog.EntityServices.ControllerContext;

/// <summary>
/// Controller context interface.
/// </summary>
public interface IControllerContext
{
    /// <summary>
    /// Gets or sets current user.
    /// </summary>
    ApplicationUser CurrentUser { get; set; }
}