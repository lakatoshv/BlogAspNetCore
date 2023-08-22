// <copyright file="JwtFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Identity.Auth;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Blog.Core;
using Data.Models;
using Utilities;

/// <summary>
/// Jwt factory.
/// </summary>
public class JwtFactory : IJwtFactory
{
    /// <summary>
    /// Jwt issuer options.
    /// </summary>
    private readonly JwtIssuerOptions jwtOptions;

    /// <summary>
    /// User manager.
    /// </summary>
    private readonly UserManager<ApplicationUser> userManager;

    /// <summary>
    /// Role manager.
    /// </summary>
    private readonly RoleManager<ApplicationRole> roleManager;

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
        this.jwtOptions = jwtOptions.Value;
        this.userManager = userManager;
        this.roleManager = roleManager;
        ThrowIfInvalidOptions(this.jwtOptions);
    }

    /// <inheritdoc cref="IJwtFactory"/>
    public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity)
    {
        var claims = new List<Claim>(new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userName),
            new Claim(JwtRegisteredClaimNames.Jti, await this.jwtOptions.JtiGenerator()),
            new Claim(
                JwtRegisteredClaimNames.Iat,
                this.jwtOptions.IssuedAt.ToUnixTimeStamp().ToString(),
                ClaimValueTypes.Integer64),

            identity.FindFirst(JwtClaimTypes.Rol),
            identity.FindFirst(JwtClaimTypes.Id),
            identity.FindFirst(JwtClaimTypes.UserName),
            identity.FindFirst(JwtClaimTypes.Email),
            identity.FindFirst(JwtClaimTypes.PhoneNumber),
            identity.FindFirst(JwtClaimTypes.IsEmailVerified),
            identity.FindFirst(JwtClaimTypes.ProfileId),
        });

        var user = await this.userManager.FindByNameAsync(userName);

        claims.AddRange(await this.userManager.GetClaimsAsync(user));

        var roleNames = await this.userManager.GetRolesAsync(user);
        foreach (var roleName in roleNames)
        {
            // Find IdentityRole by name
            var role = await this.roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                // Convert Identity to claim and add
                var roleClaim = new Claim("roles", role.Name, ClaimValueTypes.String, this.jwtOptions.Issuer);
                claims.Add(roleClaim);

                // Add claims belonging to the role
                var roleClaims = await this.roleManager.GetClaimsAsync(role);
                claims.AddRange(roleClaims);
            }
        }

        // Create the JWT security token and encode it.
        var jwt = new JwtSecurityToken(
            issuer: this.jwtOptions.Issuer,
            audience: this.jwtOptions.Audience,
            claims: claims,
            notBefore: this.jwtOptions.NotBefore,
            expires: this.jwtOptions.Expiration,
            signingCredentials: this.jwtOptions.SigningCredentials);

        var jwtHandler = new JwtSecurityTokenHandler();
        var encodedJwt = jwtHandler.WriteToken(jwt);

        return encodedJwt;
    }

    /// <inheritdoc cref="IJwtFactory"/>
    public ClaimsIdentity GenerateClaimsIdentity(ClaimsIdentityUserModel claimsIdentityUserModel)
    {
        var claims = new[]
        {
            // TODO: Consider refactoring using nameof()
            new Claim(JwtClaimTypes.Id, claimsIdentityUserModel.Id),
            new Claim(JwtClaimTypes.Rol, JwtClaims.ApiAccess),
            new Claim(JwtClaimTypes.UserName, claimsIdentityUserModel.UserName),
            new Claim(JwtClaimTypes.Email, claimsIdentityUserModel.Email),
            new Claim(JwtClaimTypes.PhoneNumber, claimsIdentityUserModel.PhoneNumber ?? string.Empty),
            new Claim(JwtClaimTypes.IsEmailVerified, claimsIdentityUserModel.IsEmailVerified.ToString()),
            new Claim(JwtClaimTypes.ProfileId, claimsIdentityUserModel.ProfileId.ToString()),
        };

        return new ClaimsIdentity(
            new GenericIdentity(claimsIdentityUserModel.Email, "Token"),
            claims);
    }

    /// <inheritdoc cref="IJwtFactory"/>
    public async Task<string> GenerateRefreshToken(string userName)
    {
        // change
        var user = await this.userManager.FindByNameAsync(userName);
        string refreshToken;
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            refreshToken = Convert.ToBase64String(randomNumber);
        }

        user.RefreshTokens.Add(new RefreshToken { Token = refreshToken, User = user });

        await this.userManager.UpdateAsync(user);
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