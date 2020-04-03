// <copyright file="RefreshTokenService.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Services.Identity.RefreshToken
{
    using System.Linq;
    using System.Threading.Tasks;
    using Auth;
    using Blog.Services.Core.Identity.Auth;
    using Data.Models;
    using Data.Repository;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Refresh token service.
    /// </summary>
    public class RefreshTokenService : IRefreshTokenService
    {
        /// <summary>
        /// User manager.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Jwt factory.
        /// </summary>
        private readonly IJwtFactory _jwtFactory;

        /// <summary>
        /// Jwt issuer options.
        /// </summary>
        private readonly JwtIssuerOptions _jwtOptions;

        /// <summary>
        /// Repository for RefreshToken.
        /// </summary>
        private readonly IRepository<RefreshToken> _refreshTokenRepository;

        /// <summary>
        /// Auth service.
        /// </summary>
        private readonly IAuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenService"/> class.
        /// </summary>
        /// <param name="jwtFactory">jwtFactory.</param>
        /// <param name="jwtOptions">jwtOptions.</param>
        /// <param name="refreshTokenRepository">refreshTokenRepository.</param>
        /// <param name="authService">authService.</param>
        /// <param name="userManager">userManager.</param>
        public RefreshTokenService(
            IJwtFactory jwtFactory,
            IOptions<JwtIssuerOptions> jwtOptions,
            IRepository<RefreshToken> refreshTokenRepository,
            IAuthService authService,
            UserManager<ApplicationUser> userManager)
        {
            this._jwtOptions = jwtOptions.Value;
            this._jwtFactory = jwtFactory;
            this._refreshTokenRepository = refreshTokenRepository;
            this._authService = authService;
            this._userManager = userManager;
        }

        /// <inheritdoc/>
        public async Task<string> RefreshTokensAsync(string userName, string refreshToken)
        {
            // var user =
            await this._userManager.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.UserName == userName);
            var identity = await this._authService.GetClaimsIdentityWithoutPassword(userName);
            var token = this._refreshTokenRepository.Table.Include(u => u.User).FirstOrDefault(t => t.Token.Equals(refreshToken));
            if (token == null)
            {
                return null;
            }

            this._refreshTokenRepository.Delete(token);

            return await Tokens.GenerateJwt(identity, this._jwtFactory, userName, this._jwtOptions);
        }

        /// <inheritdoc/>
        public async Task RemoveRefreshTokensAsync(string userName)
        {
            this._refreshTokenRepository.Table.Include(r => r.User);
            var tokensToDelete = await this._refreshTokenRepository.Table.Where(u => u.User.UserName == userName).ToListAsync();
            this._refreshTokenRepository.Delete(tokensToDelete);
        }
    }
}
