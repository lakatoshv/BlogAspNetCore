// <copyright file="IAuthService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Security.Claims;
using System.Threading.Tasks;
using Blog.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Blog.EntityServices.Identity.Auth;

/// <summary>
/// Auth service.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Get jwt from database.
    /// </summary>
    /// <param name="username">username.</param>
    /// <param name="password">password.</param>
    /// <returns>string.</returns>
    string GetJwt(string username, string password);

    /// <summary>
    /// Async get jwt from database.
    /// </summary>
    /// <param name="username">username.</param>
    /// <param name="password">password.</param>
    /// <returns>Task.</returns>
    Task<string> GetJwtAsync(string username, string password);

    /// <summary>
    /// Async refresh token.
    /// </summary>
    /// <param name="username">username.</param>
    /// <returns>Task.</returns>
    Task<string> RefreshTokenAsync(string username);

    /// <summary>
    /// Get user by user name.
    /// </summary>
    /// <param name="username">username.</param>
    /// <returns>Task.</returns>
    Task<ApplicationUser> GetByUserNameAsync(string username);

    /// <summary>
    /// Async verify two factor token.
    /// </summary>
    /// <param name="username">username.</param>
    /// <param name="authenticatorCode">authenticatorCode.</param>
    /// <returns>Task.</returns>
    Task<bool> VerifyTwoFactorTokenAsync(string username, string authenticatorCode);

    /// <summary>
    /// Async redeem two factor recovery code.
    /// </summary>
    /// <param name="username">username.</param>
    /// <param name="code">code.</param>
    /// <returns>Task.</returns>
    Task<IdentityResult> RedeemTwoFactorRecoveryCodeAsync(string username, string code);

    /// <summary>
    /// Get claims identity without password.
    /// </summary>
    /// <param name="userName">userName.</param>
    /// <returns>Task.</returns>
    Task<ClaimsIdentity> GetClaimsIdentityWithoutPassword(string userName);
}