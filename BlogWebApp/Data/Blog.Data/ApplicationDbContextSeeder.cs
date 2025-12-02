// <copyright file="ApplicationDbContextSeeder.cs" company="Blog">
// Copyright (c) BLog. All rights reserved.
// </copyright>

using static System.ArgumentNullException;

namespace Blog.Data;

using Blog.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System;
using System.Linq;

/// <summary>
/// Application database context seeder.
/// </summary>
public static class ApplicationDbContextSeeder
{
    /// <summary>
    /// Seed data.
    /// </summary>
    /// <param name="dbContext">dbContext.</param>
    /// <param name="serviceProvider">serviceProvider.</param>
    public static void Seed(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        ThrowIfNull(nameof(dbContext));
        ThrowIfNull(nameof(serviceProvider));

        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        Seed(dbContext, roleManager);
    }

    /// <summary>
    /// Seed data.
    /// </summary>
    /// <param name="dbContext">dbContext.</param>
    /// <param name="roleManager">roleManager.</param>
    public static void Seed(ApplicationDbContext dbContext, RoleManager<ApplicationRole> roleManager)
    {
        ThrowIfNull(nameof(dbContext));
        ThrowIfNull(nameof(roleManager));

        SeedRoles(roleManager);
    }

    /// <summary>
    /// Seed roles.
    /// </summary>
    /// <param name="roleManager">roleManager.</param>
    private static void SeedRoles(RoleManager<ApplicationRole> roleManager)
    {
        SeedRole(GlobalConstants.AdministratorRoleName, roleManager);
    }

    /// <summary>
    /// Seed role.
    /// </summary>
    /// <param name="roleName">roleName.</param>
    /// <param name="roleManager">roleManager.</param>
    private static void SeedRole(string roleName, RoleManager<ApplicationRole> roleManager)
    {
        var role = roleManager.FindByNameAsync(roleName).GetAwaiter().GetResult();
        if (role != null)
        {
            return;
        }

        var result = roleManager.CreateAsync(new ApplicationRole(roleName)).GetAwaiter().GetResult();

        if (!result.Succeeded)
        {
            throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
        }
    }
}