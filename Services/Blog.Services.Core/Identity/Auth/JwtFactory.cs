// <copyright file="JwtClaimTypes.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Identity.Auth
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using Blog.Core;
    using Data.Models;
    using Utilities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Jwt factory.
    /// </summary>
    public class JwtFactory : IJwtFactory
    {
        /// <summary>
        /// Jwt issuer options.
        /// </summary>
        private readonly JwtIssuerOptions _jwtOptions;

        /// <summary>
        /// User manager.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Role manager.
        /// </summary>
        private readonly RoleManager<ApplicationRole> _roleManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtFactory"/> class.
        /// </summary>
        /// <param name="jwtOptions">jwtOptions.</param>
        /// <param name="userManager">userManager.</param>
        /// <param name="roleManager">roleManager.</param>
        public JwtFactory(
            IOptions<JwtIssuerOptions> jwtOptions,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            this._jwtOptions = jwtOptions.Value;
            this._userManager = userManager;
            this._roleManager = roleManager;
            ThrowIfInvalidOptions(this._jwtOptions);
        }

        /// <inheritdoc cref="IJwtFactory"/>
        public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity)
        {
            var claims = new List<Claim>(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, await this._jwtOptions.JtiGenerator()),
                new Claim(
                    JwtRegisteredClaimNames.Iat,
                    this._jwtOptions.IssuedAt.ToUnixTimeStamp().ToString(),
                    ClaimValueTypes.Integer64),

                identity.FindFirst(JwtClaimTypes.Rol),
                identity.FindFirst(JwtClaimTypes.Id),
                identity.FindFirst(JwtClaimTypes.UserName),
                identity.FindFirst(JwtClaimTypes.Email),
                identity.FindFirst(JwtClaimTypes.PhoneNumber),
                identity.FindFirst(JwtClaimTypes.IsEmailVerified),
                identity.FindFirst(JwtClaimTypes.ProfileId),
            });

            var user = await this._userManager.FindByNameAsync(userName);

            claims.AddRange(await this._userManager.GetClaimsAsync(user));

            var roleNames = await this._userManager.GetRolesAsync(user);
            foreach (var roleName in roleNames)
            {
                // Find IdentityRole by name
                var role = await this._roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    // Convert Identity to claim and add
                    var roleClaim = new Claim("roles", role.Name, ClaimValueTypes.String, this._jwtOptions.Issuer);
                    claims.Add(roleClaim);

                    // Add claims belonging to the role
                    var roleClaims = await this._roleManager.GetClaimsAsync(role);
                    claims.AddRange(roleClaims);
                }
            }

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: this._jwtOptions.Issuer,
                audience: this._jwtOptions.Audience,
                claims: claims,
                notBefore: this._jwtOptions.NotBefore,
                expires: this._jwtOptions.Expiration,
                signingCredentials: this._jwtOptions.SigningCredentials);

            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;
        }

        /// <inheritdoc cref="IJwtFactory"/>
        public ClaimsIdentity GenerateClaimsIdentity(ClaimsIdentityUserModel claimsIdentityUserModel)
        {
            return new ClaimsIdentity(
                new GenericIdentity(claimsIdentityUserModel.Email, "Token"),
                new[]
                {
                    // TODO: Consider refactoring using nameof()
                    new Claim(JwtClaimTypes.Id, claimsIdentityUserModel.Id),
                    new Claim(JwtClaimTypes.Rol, JwtClaims.ApiAccess),
                    new Claim(JwtClaimTypes.UserName, claimsIdentityUserModel.UserName),
                    new Claim(JwtClaimTypes.Email, claimsIdentityUserModel.Email),
                    new Claim(JwtClaimTypes.PhoneNumber, claimsIdentityUserModel.PhoneNumber ?? string.Empty),
                    new Claim(JwtClaimTypes.IsEmailVerified, claimsIdentityUserModel.IsEmailVerified.ToString()),
                    new Claim(JwtClaimTypes.ProfileId, claimsIdentityUserModel.ProfileId.ToString()),
                });
        }

        /// <inheritdoc cref="IJwtFactory"/>
        public async Task<string> GenerateRefreshToken(string userName)
        {
            // change
            var user = await this._userManager.FindByNameAsync(userName);
            string refreshToken;
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken = Convert.ToBase64String(randomNumber);
            }

            user.RefreshTokens.Add(new RefreshToken { Token = refreshToken, User = user });

            await this._userManager.UpdateAsync(user);
            return refreshToken;
        }

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }
    }
}
