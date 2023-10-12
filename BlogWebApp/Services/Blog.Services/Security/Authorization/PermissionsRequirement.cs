// <copyright file="PermissionsRequirement.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;

namespace Blog.EntityServices.Security.Authorization;

/// <summary>
/// Permissions requirement.
/// </summary>
/// <seealso cref="IAuthorizationRequirement" />
/// <remarks>
/// Initializes a new instance of the <see cref="PermissionsRequirement"/> class.
/// </remarks>
/// <param name="permissionSystemName">Name of the permission system.</param>
public class PermissionsRequirement(string permissionSystemName)
    : IAuthorizationRequirement
{
    /// <summary>
    /// Gets the name of the permission system.
    /// </summary>
    /// <value>
    /// The name of the permission system.
    /// </value>
    public string PermissionSystemName { get; } = permissionSystemName;
}