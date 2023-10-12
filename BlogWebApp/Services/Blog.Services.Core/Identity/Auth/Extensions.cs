// <copyright file="Extensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Identity.Auth;

using System.Linq;
using System.Security.Claims;

/// <summary>
/// Extensions.
/// </summary>
internal static class Extensions
{
    /// <summary>
    /// GetId.
    /// </summary>
    /// <param name="identity">identity.</param>
    /// <returns>string.</returns>
    public static string GetId(this ClaimsIdentity identity)
    {
        var id = identity.Claims.Single(c => c.Type == JwtClaimTypes.Id).Value;

        return id;
    }
}