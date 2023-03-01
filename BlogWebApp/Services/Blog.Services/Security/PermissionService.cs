// <copyright file="PermissionService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Security
{
    using Blog.Data.Models;
    using Blog.Services.ControllerContext;
    using Blog.Services.Core.Caching;
    using Blog.Services.Core.Caching.Interfaces;
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Permission service.
    /// </summary>
    public class PermissionService : IPermissionService
    {
        /// <summary>
        /// The work context.
        /// </summary>
        private readonly IControllerContext workContext;

        /// <summary>
        /// The cache manager.
        /// </summary>
        private readonly IStaticCacheManager cacheManager;

        /// <summary>
        /// The user manager.
        /// </summary>
        private readonly UserManager<ApplicationUser> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionService"/> class.
        /// </summary>
        /// <param name="workContext">The work context.</param>
        /// <param name="cacheManager">The cache manager.</param>
        /// <param name="userManager">The user manager.</param>
        public PermissionService(
            IControllerContext workContext,
            IStaticCacheManager cacheManager,
            UserManager<ApplicationUser> userManager)
        {
            this.workContext = workContext;
            this.cacheManager = cacheManager;
            this.userManager = userManager;
        }

        /// <inheritdoc cref="IPermissionService" />
        public bool Authorize()
        {
            return this.Authorize(this.workContext.CurrentUser);
        }

        private bool Authorize(ApplicationUser user)
        {
            if (this.CheckIsUserAdmin(user))
            {
                return true;
            }

            return true;
        }

        private bool CheckIsUserAdmin(ApplicationUser user)
        {
            string systemAdminKey = string.Format("Admin", user.Id);
            return this.cacheManager.Get(systemAdminKey, () => this.userManager.IsInRoleAsync(user, "Admin").Result);
        }
    }
}
