// <copyright file="ApplicationRoleStore.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Data
{
    using System.Security.Claims;
    using Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    /// <summary>
    /// Application role store.
    /// </summary>
    public class ApplicationRoleStore : RoleStore<
        ApplicationRole,
        ApplicationDbContext,
        string,
        IdentityUserRole<string>,
        IdentityRoleClaim<string>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRoleStore"/> class.
        /// </summary>
        /// <param name="context">context.</param>
        /// <param name="describer">describer.</param>
        public ApplicationRoleStore(ApplicationDbContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
        }

        /// <summary>
        /// Create role claim.
        /// </summary>
        /// <param name="role">role.</param>
        /// <param name="claim">claim.</param>
        /// <returns>IdentityRoleClaim.</returns>
        protected override IdentityRoleClaim<string> CreateRoleClaim(ApplicationRole role, Claim claim) =>
            new IdentityRoleClaim<string>
            {
                RoleId = role.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
            };
    }
}
