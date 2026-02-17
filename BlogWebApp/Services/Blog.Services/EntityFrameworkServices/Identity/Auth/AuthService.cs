// <copyright file="AuthService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.EntityServices.EntityFrameworkServices.Identity.Auth;

using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core;
using Data.Models;
using Data.Specifications;
using Blog.Services.Core.Identity.Auth;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

/// <summary>
/// Auth service.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="AuthService"/> class.
/// </remarks>
/// <param name="userManager">userManager.</param>
/// <param name="jwtFactory">jwtFactory.</param>
/// <param name="jwtOptions">jwtOptions.</param>
/// <param name="profileService">profileService.</param>
public class AuthService(
    UserManager<ApplicationUser> userManager,
    IJwtFactory jwtFactory,
    IOptions<JwtIssuerOptions> jwtOptions,
    IProfileService profileService)
    : IAuthService
{
    // TODO: Add another abstraction level against UserManager
    // so it can be replaced with non identity implementation

    /// <summary>
    /// User manager.
    /// </summary>
    private readonly UserManager<ApplicationUser> userManager = userManager;

    /// <summary>
    /// Jwt factory.
    /// </summary>
    private readonly IJwtFactory jwtFactory = jwtFactory;

    /// <summary>
    /// Jwt issuer options.
    /// </summary>
    private readonly JwtIssuerOptions jwtOptions = jwtOptions.Value;

    /// <summary>
    /// The profile service.
    /// </summary>
    private readonly IProfileService profileService = profileService;

    /// <inheritdoc cref="IAuthService"/>
    // TODO: Either implement or remove
    public string GetJwt(string username, string password)
        => throw new NotImplementedException();

    /// <inheritdoc cref="IAuthService"/>
    public async Task<ApplicationUser> GetByUserNameAsync(string username)
        => await this.userManager.Users
            .Where(x => x.Email.Equals(username))
            .FirstOrDefaultAsync();

    /// <inheritdoc cref="IAuthService"/>
    public async Task<bool> VerifyTwoFactorTokenAsync(string username, string authenticatorCode)
    {
        var user = await this.GetByUserNameAsync(username);

        return await this.userManager.VerifyTwoFactorTokenAsync(
            user, this.userManager.Options.Tokens.AuthenticatorTokenProvider, authenticatorCode);
    }

    /// <inheritdoc cref="IAuthService"/>
    public async Task<IdentityResult> RedeemTwoFactorRecoveryCodeAsync(string username, string code)
    {
        var user = await this.GetByUserNameAsync(username);

        return await this.userManager.RedeemTwoFactorRecoveryCodeAsync(user, code);
    }

    /// <inheritdoc cref="IAuthService"/>
    public async Task<string> GetJwtAsync(string username, string password)
    {
        var identity = await this.GetClaimsIdentity(username, password);
        if (identity == null)
        {
            return null;
        }

        var jwt = await Tokens.GenerateJwt(identity, this.jwtFactory, username, this.jwtOptions);

        return jwt;
    }

    /// <inheritdoc cref="IAuthService"/>
    public async Task<string> RefreshTokenAsync(string username)
    {
        var identity = await this.GetClaimsIdentityWithoutPassword(username);
        if (identity == null)
        {
            return null;
        }

        return await Tokens.GenerateJwt(identity, this.jwtFactory, username, this.jwtOptions);
    }

    /// <inheritdoc cref="IAuthService"/>
    public async Task<ClaimsIdentity> GetClaimsIdentityWithoutPassword(string userName)
    {
        if (string.IsNullOrEmpty(userName))
        {
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        // get the user to verify
        var userToVerify = await this.userManager.FindByNameAsync(userName);

        if (userToVerify == null)
        {
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        userToVerify.Profile = this.profileService.FirstOrDefault(new ProfileSpecification(x => x.UserId.Equals(userToVerify.Id)));

        var claimsIdentityUserModel = GetIdentityClaims(userToVerify, userName);

        return await Task.FromResult(this.jwtFactory.GenerateClaimsIdentity(claimsIdentityUserModel));
    }

    /// <summary>
    /// Get identity claims.
    /// </summary>
    /// <param name="userToVerify">userToVerify.</param>
    /// <param name="userName">userName.</param>
    /// <returns>ClaimsIdentityUserModel.</returns>
    private static ClaimsIdentityUserModel GetIdentityClaims(ApplicationUser userToVerify, string userName)
        => new ()
        {
            Id = userToVerify.Id,
            Email = userName,
            UserName = userToVerify.UserName,
            PhoneNumber = userToVerify.PhoneNumber,
            IsEmailVerified = userToVerify.EmailConfirmed,
            ProfileId = userToVerify.Profile != null ? userToVerify.Profile.Id : 1,
        };

    /// <summary>
    /// Get claims identity.
    /// </summary>
    /// <param name="userName">userName.</param>
    /// <param name="password">password.</param>
    /// <returns>Task.</returns>
    private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
    {
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
        {
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        // get the user to verify
        var userToVerify = await this.userManager.FindByNameAsync(userName);

        if (userToVerify == null)
        {
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        userToVerify.Profile = this.profileService.FirstOrDefault(new ProfileSpecification(x => x.UserId.Equals(userToVerify.Id)));

        var claimsIdentityUserModel = GetIdentityClaims(userToVerify, userName);

        // check the credentials
        if (await this.userManager.CheckPasswordAsync(userToVerify, password))
        {
            return await Task.FromResult(this.jwtFactory.GenerateClaimsIdentity(claimsIdentityUserModel));
        }

        // Credentials are invalid, or account doesn't exist
        return await Task.FromResult<ClaimsIdentity>(null);
    }
}