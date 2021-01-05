// <copyright file="ApplicationDbContextSeeder.cs" company="Blog">
// Copyright (c) BLog. All rights reserved.
// </copyright>

namespace Blog.Data
{
    using System;
    using System.Linq;
    using Blog.Core;
    using Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

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
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

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
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (roleManager == null)
            {
                throw new ArgumentNullException(nameof(roleManager));
            }

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
            if (role == null)
            {
                var result = roleManager.CreateAsync(new ApplicationRole(roleName)).GetAwaiter().GetResult();

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
