// <copyright file="IControllerContext.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.EntityServices.ControllerContext;

using Data.Models;

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