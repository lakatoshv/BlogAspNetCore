// <copyright file="IUserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.EntityServices.EntityFrameworkServices.Identity.User;

using Contracts.V1.Responses.Chart;
using Core.Infrastructure.Pagination;
using Core.TableFilters;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// User service interface.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Async get all filtered users.
    /// </summary>
    /// <param name="tableFilter">tableFilter.</param>
    /// <returns>Task.</returns>
    Task<PagedListResult<ApplicationUser>> GetAllFilteredUsersAsync(TableFilter tableFilter);

    /// <summary>
    /// Async et all users.
    /// </summary>
    /// <returns>Task.</returns>
    Task<List<ApplicationUser>> GetAllAsync();

    /// <summary>
    /// Async create user.
    /// </summary>
    /// <param name="user">user.</param>
    /// <param name="password">password.</param>
    /// <returns>Task.</returns>
    Task<IdentityResult> CreateAsync(ApplicationUser user, string password);

    /// <summary>
    /// Async delete user by id.
    /// </summary>
    /// <param name="id">id.</param>
    /// <returns>Task.</returns>
    Task DelByIdAsync(string id);

    /// <summary>
    /// Async get user by name.
    /// </summary>
    /// <param name="userName">userName.</param>
    /// <returns>Task.</returns>
    Task<ApplicationUser> GetByUserNameAsync(string userName);

    /// <summary>
    /// Get user by profile id.
    /// </summary>
    /// <param name="profileId">profileId.</param>
    /// <returns>Task.</returns>
    Task<ApplicationUser> GetByProfileIdAsync(int profileId);

    /// <summary>
    /// Async get user by email.
    /// </summary>
    /// <param name="email">email.</param>
    /// <returns>Task.</returns>
    Task<ApplicationUser> GetByEmailAsync(string email);

    /// <summary>
    /// Get user by id.
    /// </summary>
    /// <param name="id">id.</param>
    /// <returns>Task.</returns>
    Task<ApplicationUser> GetByIdAsync(string id);

    /// <summary>
    /// Async gret users by ids.
    /// </summary>
    /// <param name="userIds">userIds.</param>
    /// <returns>Task.</returns>
    Task<IList<ApplicationUser>> GetUsersAsync(IEnumerable<string> userIds);

    /// <summary>
    /// Async change password.
    /// </summary>
    /// <param name="userName">userName.</param>
    /// <param name="oldPassword">oldPassword.</param>
    /// <param name="newPassword">newPassword.</param>
    /// <returns>Task.</returns>
    Task<IdentityResult> ChangePasswordAsync(string userName, string oldPassword, string newPassword);

    /// <summary>
    /// Async get email verification token.
    /// </summary>
    /// <param name="userName">userName.</param>
    /// <returns>Task.</returns>
    Task<string> GetEmailVerificationTokenAsync(string userName);

    /// <summary>
    /// Async get password reset token.
    /// </summary>
    /// <param name="userName">userName.</param>
    /// <returns>Task.</returns>
    Task<string> GetPasswordResetTokenAsync(string userName);

    /// <summary>
    /// Async get authenticator key.
    /// </summary>
    /// <param name="userName">userName.</param>
    /// <returns>Task.</returns>
    Task<string> GetAuthenticatorKeyAsync(string userName);

    /// <summary>
    /// Async get authenticator key.
    /// </summary>
    /// <param name="user">user.</param>
    /// <returns>Task.</returns>
    Task<string> GetAuthenticatorKeyAsync(ApplicationUser user);

    /// <summary>
    /// Async reset authenticator key.
    /// </summary>
    /// <param name="user">user.</param>
    /// <returns>Task.</returns>
    Task<IdentityResult> ResetAuthenticatorKeyAsync(ApplicationUser user);

    /// <summary>
    /// Async count recovery codes.
    /// </summary>
    /// <param name="user">user.</param>
    /// <returns>Task.</returns>
    Task<int> CountRecoveryCodesAsync(ApplicationUser user);

    /// <summary>
    /// Async verify two factor token.
    /// </summary>
    /// <param name="user">user.</param>
    /// <param name="tokenProvider">tokenProvider.</param>
    /// <param name="token">token.</param>
    /// <returns>Task.</returns>
    Task<bool> VerifyTwoFactorTokenAsync(ApplicationUser user, string tokenProvider, string token);

    /// <summary>
    /// Get authentication provider.
    /// </summary>
    /// <returns>string.</returns>
    string GetAuthenticationProvider();

    /// <summary>
    /// Async set two factor enabled.
    /// </summary>
    /// <param name="user">user.</param>
    /// <param name="allow">allow.</param>
    /// <returns>Task.</returns>
    Task<IdentityResult> SetTwoFactorEnabledAsync(ApplicationUser user, bool allow);

    /// <summary>
    /// Async generate new two factor recovery codes.
    /// </summary>
    /// <param name="user">user.</param>
    /// <param name="number">number.</param>
    /// <returns>Task.</returns>
    Task<IEnumerable<string>> GenerateNewTwoFactorRecoveryCodesAsync(ApplicationUser user, int number);

    /// <summary>
    /// Async update user.
    /// </summary>
    /// <param name="userName">userName.</param>
    /// <param name="applicationUser">applicationUser.</param>
    /// <returns>Task.</returns>
    Task<IdentityResult> UpdateAsync(string userName, ApplicationUser applicationUser);

    /// <summary>
    /// Async verify email.
    /// </summary>
    /// <param name="userName">userName.</param>
    /// <param name="token">token.</param>
    /// <returns>Task.</returns>
    Task<IdentityResult> VerifyEmailAsync(string userName, string token);

    /// <summary>
    /// Async reset password.
    /// </summary>
    /// <param name="userName">userName.</param>
    /// <param name="token">token.</param>
    /// <param name="newPassword">newPassword.</param>
    /// <returns>Task.</returns>
    Task<IdentityResult> ResetPasswordAsync(string userName, string token, string newPassword);

    /// <summary>
    /// Asynchronous Get posts activity.
    /// </summary>
    /// <returns>Task.</returns>
    Task<ChartDataModel> GetUsersActivity();
}