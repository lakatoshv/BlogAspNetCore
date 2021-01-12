// <copyright file="ApplicationUserStore.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Data
{
    using System.Security.Claims;
    using Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    /// <summary>
    /// Application user store.
    /// </summary>
    public class ApplicationUserStore : UserStore<
        ApplicationUser,
        ApplicationRole,
        ApplicationDbContext,
        string,
        IdentityUserClaim<string>,
        IdentityUserRole<string>,
        IdentityUserLogin<string>,
        IdentityUserToken<string>,
        IdentityRoleClaim<string>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserStore"/> class.
        /// </summary>
        /// <param name="context">context.</param>
        /// <param name="describer">describer.</param>
        public ApplicationUserStore(ApplicationDbContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
        }

        /// <summary>
        /// Create user role.
        /// </summary>
        /// <param name="user">user.</param>
        /// <param name="role">role.</param>
        /// <returns>IdentityUserRole.</returns>
        protected override IdentityUserRole<string> CreateUserRole(ApplicationUser user, ApplicationRole role)
        {
            return new IdentityUserRole<string> { RoleId = role.Id, UserId = user.Id };
        }

        /// <summary>
        /// Create user claim.
        /// </summary>
        /// <param name="user">user.</param>
        /// <param name="claim">claim.</param>
        /// <returns>IdentityUserClaim.</returns>
        protected override IdentityUserClaim<string> CreateUserClaim(ApplicationUser user, Claim claim)
        {
            var identityUserClaim = new IdentityUserClaim<string> { UserId = user.Id };
            identityUserClaim.InitializeFromClaim(claim);
            return identityUserClaim;
        }

        /// <summary>
        /// Create user login.
        /// </summary>
        /// <param name="user">user.</param>
        /// <param name="login">login.</param>
        /// <returns>IdentityUserLogin.</returns>
        protected override IdentityUserLogin<string> CreateUserLogin(ApplicationUser user, UserLoginInfo login) =>
            new IdentityUserLogin<string>
            {
                UserId = user.Id,
                ProviderKey = login.ProviderKey,
                LoginProvider = login.LoginProvider,
                ProviderDisplayName = login.ProviderDisplayName,
            };

        /// <summary>
        /// Create user token.
        /// </summary>
        /// <param name="user">user.</param>
        /// <param name="loginProvider">loginProvider.</param>
        /// <param name="name">name.</param>
        /// <param name="value">value.</param>
        /// <returns>IdentityUserToken.</returns>
        protected override IdentityUserToken<string> CreateUserToken(
            ApplicationUser user,
            string loginProvider,
            string name,
            string value)
        {
            var token = new IdentityUserToken<string>
            {
                UserId = user.Id,
                LoginProvider = loginProvider,
                Name = name,
                Value = value,
            };
            return token;
        }
    }
}
