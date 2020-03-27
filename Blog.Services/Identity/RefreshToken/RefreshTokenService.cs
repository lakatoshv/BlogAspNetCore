// <copyright file="RefreshTokenService.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Services.Identity.RefreshToken
{
    using System.Linq;
    using System.Threading.Tasks;
    using Blog.Data.Models;
    using Blog.Data.Repository;
    using Blog.Services.Core.Identity.Auth;
    using Blog.Services.Identity.Auth;
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
        private readonly UserManager<ApplicationUser> userManager;

        /// <summary>
        /// Jwt factory.
        /// </summary>
        private readonly IJwtFactory jwtFactory;

        /// <summary>
        /// Jwt issuer options.
        /// </summary>
        private readonly JwtIssuerOptions jwtOptions;

        /// <summary>
        /// Repository for RefreshToken.
        /// </summary>
        private readonly IRepository<RefreshToken> refreshTokenRepository;

        /// <summary>
        /// Auth service.
        /// </summary>
        private readonly IAuthService authService;

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
            IRepository<Data.Models.RefreshToken> refreshTokenRepository,
            IAuthService authService,
            UserManager<ApplicationUser> userManager)
        {
            this.jwtOptions = jwtOptions.Value;
            this.jwtFactory = jwtFactory;
            this.refreshTokenRepository = refreshTokenRepository;
            this.authService = authService;
            this.userManager = userManager;
        }

        /// <inheritdoc/>
        public async Task<string> RefreshTokensAsync(string userName, string refreshToken)
        {
            var user = await this.userManager.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.UserName == userName);
            var identity = await this.authService.GetClaimsIdentityWithoutPassword(userName);
            var token = this.refreshTokenRepository.Table.Include(u => u.User).FirstOrDefault(t => t.Token.Equals(refreshToken));
            if (token == null)
            {
                return null;
            }

            this.refreshTokenRepository.Delete(token);

            return await Tokens.GenerateJwt(identity, this.jwtFactory, userName, this.jwtOptions);
        }

        /// <inheritdoc/>
        public async Task RemoveRefreshTokensAsync(string userName)
        {
            this.refreshTokenRepository.Table.Include(r => r.User);
            var tokensToDelete = await this.refreshTokenRepository.Table.Where(u => u.User.UserName == userName).ToListAsync();
            this.refreshTokenRepository.Delete(tokensToDelete);
        }
    }
}
