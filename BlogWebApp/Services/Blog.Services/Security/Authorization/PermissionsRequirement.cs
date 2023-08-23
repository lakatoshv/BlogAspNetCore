// <copyright file="PermissionsRequirement.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Security.Authorization;

using Microsoft.AspNetCore.Authorization;

/// <summary>
/// Permissions requirement.
/// </summary>
/// <seealso cref="IAuthorizationRequirement" />
public class PermissionsRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// Gets the name of the permission system.
    /// </summary>
    /// <value>
    /// The name of the permission system.
    /// </value>
    public string PermissionSystemName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionsRequirement"/> class.
    /// </summary>
    /// <param name="permissionSystemName">Name of the permission system.</param>
    public PermissionsRequirement(string permissionSystemName) => this.PermissionSystemName = permissionSystemName;
}