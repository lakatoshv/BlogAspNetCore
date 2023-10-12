// <copyright file="ApplicationRoleStore.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Data;

using System.Security.Claims;
using Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

/// <summary>
/// Application role store.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ApplicationRoleStore"/> class.
/// </remarks>
/// <param name="context">context.</param>
/// <param name="describer">describer.</param>
public class ApplicationRoleStore(ApplicationDbContext context, IdentityErrorDescriber describer = null)
    : RoleStore<
        ApplicationRole,
        ApplicationDbContext,
        string,
        IdentityUserRole<string>,
        IdentityRoleClaim<string>>(context, describer)
{
    /// <summary>
    /// Create role claim.
    /// </summary>
    /// <param name="role">role.</param>
    /// <param name="claim">claim.</param>
    /// <returns>IdentityRoleClaim.</returns>
    protected override IdentityRoleClaim<string> CreateRoleClaim(ApplicationRole role, Claim claim) =>
        new ()
        {
            RoleId = role.Id,
            ClaimType = claim.Type,
            ClaimValue = claim.Value,
        };
}