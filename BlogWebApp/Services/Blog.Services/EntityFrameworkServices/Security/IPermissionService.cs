// <copyright file="IPermissionService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.EntityServices.EntityFrameworkServices.Security;

/// <summary>
/// Permission service interface.
/// </summary>
public interface IPermissionService
{
    /// <summary>
    /// Authorize permission.
    /// </summary>
    /// <returns>bool.</returns>
    bool Authorize();
}