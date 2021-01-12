// <copyright file="IJwtFactory.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Identity.Auth
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Blog.Core;

    /// <summary>
    /// Jwt factory interface.
    /// </summary>
    public interface IJwtFactory
    {
        /// <summary>
        /// Generate encoded token.
        /// </summary>
        /// <param name="userName">userName.</param>
        /// <param name="identity">identity.</param>
        /// <returns>Task.</returns>
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);

        /// <summary>
        /// Generate refresh token.
        /// </summary>
        /// <param name="userName">userName.</param>
        /// <returns>Task.</returns>
        Task<string> GenerateRefreshToken(string userName);

        /// <summary>
        /// Generate claims identity.
        /// </summary>
        /// <param name="claimsIdentityUserModel">claimsIdentityUserModel.</param>
        /// <returns>ClaimsIdentity.</returns>
        ClaimsIdentity GenerateClaimsIdentity(ClaimsIdentityUserModel claimsIdentityUserModel);
    }
}
