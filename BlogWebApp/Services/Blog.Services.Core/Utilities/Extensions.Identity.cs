// <copyright file="Extensions.Identity.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Utilities;

using System.Security.Claims;

/// <summary>
/// Identity extensions.
/// </summary>
public static partial class Extensions
{
    /// <summary>
    /// Get user name.
    /// </summary>
    /// <param name="identity">identity.</param>
    /// <returns>string.</returns>
    public static string GetUserName(this ClaimsPrincipal identity)
    {
        var username = identity.FindFirst(ClaimTypes.NameIdentifier);

        return username?.Value;
    }
}