// <copyright file="IRefreshTokenService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Identity.RefreshToken
{
    using System.Threading.Tasks;

    /// <summary>
    /// Refresh token service interface.
    /// </summary>
    public interface IRefreshTokenService
    {
        /// <summary>
        /// Async refresh tokens.
        /// </summary>
        /// <param name="userName">userName.</param>
        /// <param name="refreshToken">refreshToken.</param>
        /// <returns>Task.</returns>
        Task<string> RefreshTokensAsync(string userName, string refreshToken);

        /// <summary>
        /// Async remove refresh tokens.
        /// </summary>
        /// <param name="userName">userName.</param>
        /// <returns>Task.</returns>
        Task RemoveRefreshTokensAsync(string userName);
    }
}
