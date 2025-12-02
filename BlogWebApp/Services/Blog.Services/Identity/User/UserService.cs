// <copyright file="UserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.EntityServices.Identity.User;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.V1.Responses.Chart;
using Core.Infrastructure.Pagination;
using Core.TableFilters;
using Data.Models;
using Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// User service.
/// </summary>
public class UserService : IUserService
{
    /// <summary>
    /// User manager.
    /// </summary>
    private readonly UserManager<ApplicationUser> userManager;

    /// <summary>
    /// Repository for ApplicationUser.
    /// </summary>
    private readonly IRepository<ApplicationUser> applicationUserRepository;

    /// <summary>
    /// Mapper.
    /// </summary>
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="userManager">userManager.</param>
    /// <param name="applicationUserRepository">applicationUserRepository.</param>
    /// <param name="mapper">mapper.</param>
    public UserService(
        UserManager<ApplicationUser> userManager,
        IRepository<ApplicationUser> applicationUserRepository,
        IMapper mapper)
    {
        this.userManager = userManager;
        this.applicationUserRepository = applicationUserRepository;
        this.mapper = mapper;
    }

    /// <inheritdoc cref="IUserService"/>
    public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
    {
        var result = await this.userManager.CreateAsync(user, password);

        return result;
    }

    /// <inheritdoc cref="IUserService"/>
    public async Task<ApplicationUser> GetByIdAsync(string id)
    {
        var user = await this.userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

        return user;
    }

    /// <inheritdoc cref="IUserService"/>
    public async Task<IList<ApplicationUser>> GetUsersAsync(IEnumerable<string> userIds)
        => await this.userManager.Users
            .Where(x => userIds.Contains(x.Id))
            .ToListAsync();

    /// <inheritdoc cref="IUserService"/>
    public async Task<ApplicationUser> GetByEmailAsync(string email)
        => await this.userManager.Users
            .FirstOrDefaultAsync(m => m.Email == email);

    /// <inheritdoc cref="IUserService"/>
    public async Task<IdentityResult> ChangePasswordAsync(string userName, string oldPassword, string newPassword)
    {
        var user = await this.GetByUserNameAsync(userName);
        var changePasswordResult = await this.userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        return changePasswordResult;
    }

    /// <inheritdoc cref="IUserService"/>
    public async Task<List<ApplicationUser>> GetAllAsync()
    {
        var userList = await this.userManager.Users.ToListAsync();
        return userList;
    }

    /// <inheritdoc cref="IUserService"/>
    public async Task<string> GetAuthenticatorKeyAsync(string userName)
    {
        var user = await this.GetByUserNameAsync(userName);
        var token = await this.userManager.GetAuthenticatorKeyAsync(user);

        return token;
    }

    /// <inheritdoc cref="IUserService"/>
    public Task<string> GetAuthenticatorKeyAsync(ApplicationUser user)
        => this.userManager.GetAuthenticatorKeyAsync(user);

    /// <inheritdoc cref="IUserService"/>
    public async Task<IdentityResult> ResetAuthenticatorKeyAsync(ApplicationUser user)
        => await this.userManager.ResetAuthenticatorKeyAsync(user);

    /// <inheritdoc cref="IUserService"/>
    public async Task<ApplicationUser> GetByUserNameAsync(string userName)
        => await this.userManager.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.UserName == userName);

    /// <inheritdoc cref="IUserService"/>
    public async Task<ApplicationUser> GetByProfileIdAsync(int profileId)
        => await this.userManager.Users
            .FirstOrDefaultAsync();

    /// <inheritdoc cref="IUserService"/>
    public async Task DelByIdAsync(string id)
    {
        var user = await this.userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user != null)
        {
            user.Email = user.Id;
            user.UserName = user.Id;

            await this.userManager.UpdateAsync(user);
        }
    }

    /// <inheritdoc cref="IUserService"/>
    public async Task<string> GetEmailVerificationTokenAsync(string userName)
    {
        var user = await this.userManager.FindByNameAsync(userName);
        var token = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(token);

        return System.Convert.ToBase64String(plainTextBytes);
    }

    /// <inheritdoc cref="IUserService"/>
    public async Task<string> GetPasswordResetTokenAsync(string userName)
    {
        var user = await this.userManager.FindByNameAsync(userName);
        var token = await this.userManager.GeneratePasswordResetTokenAsync(user);

        return token;
    }

    /// <inheritdoc cref="IUserService"/>
    public async Task<IdentityResult> UpdateAsync(string userName, ApplicationUser applicationUser)
    {
        var user = await this.userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        this.mapper.Map(applicationUser, user);

        var result = await this.userManager.UpdateAsync(user);

        return result;
    }

    /// <inheritdoc cref="IUserService"/>
    public async Task<IdentityResult> VerifyEmailAsync(string userName, string token)
    {
        var user = await this.userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);

        var base64EncodedBytes = System.Convert.FromBase64String(token);
        token = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

        var result = await this.userManager.ConfirmEmailAsync(user, token);

        return result;
    }

    /// <inheritdoc cref="IUserService"/>
    public Task<int> CountRecoveryCodesAsync(ApplicationUser user)
    {
        return this.userManager.CountRecoveryCodesAsync(user);
    }

    /// <inheritdoc cref="IUserService"/>
    public Task<bool> VerifyTwoFactorTokenAsync(ApplicationUser user, string tokenProvider, string token)
        => this.userManager.VerifyTwoFactorTokenAsync(user, tokenProvider, token);

    /// <inheritdoc cref="IUserService"/>
    public string GetAuthenticationProvider()
        => this.userManager.Options.Tokens.AuthenticatorTokenProvider;

    /// <inheritdoc cref="IUserService"/>
    public Task<IdentityResult> SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        => this.userManager.SetTwoFactorEnabledAsync(user, enabled);

    /// <inheritdoc cref="IUserService"/>
    public Task<IEnumerable<string>> GenerateNewTwoFactorRecoveryCodesAsync(ApplicationUser user, int number)
        => this.userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, number);

    /// <inheritdoc cref="IUserService"/>
    public async Task<IdentityResult> ResetPasswordAsync(string userName, string token, string newPassword)
    {
        var user = await this.GetByUserNameAsync(userName);
        var result = await this.userManager.ResetPasswordAsync(user, token, newPassword);

        return result;
    }

    /// <inheritdoc cref="IUserService"/>
    public async Task<PagedListResult<ApplicationUser>> GetAllFilteredUsersAsync(TableFilter tableFilter)
    {
        var sequence = this.applicationUserRepository.Table;

        return await this.applicationUserRepository.SearchBySequenceAsync(this.AddFilter(tableFilter), sequence);
    }

    /// <inheritdoc cref="IUserService"/>
    public async Task<ChartDataModel> GetUsersActivity()
        => new ()
        {
            Name = "Posts",
            Series = await applicationUserRepository.TableNoTracking
                .GroupBy(x => x.CreatedOn)
                .Select(x => new ChartItem
                {
                    Name = x.Key.ToString("dd/MM/yyyy"),
                    Value = x.Count(),
                })
                .ToListAsync(),
        };

    /// <summary>
    /// Add filter.
    /// </summary>
    /// <param name="tableFilter">tableFilter.</param>
    /// <returns>SearchQuery.</returns>
    private SearchQuery<ApplicationUser> AddFilter(TableFilter tableFilter)
    {
        var query = this.applicationUserRepository.GenerateQuery(tableFilter);
        var searchWord = tableFilter.Search.Value.ToUpper();

        query.AddFilter(x =>
            x.UserName.ToUpper().Contains(searchWord)
            || x.Email.ToUpper().Contains(searchWord)
            || x.UserName.ToUpper().Contains(searchWord)
            || x.PhoneNumber.Contains(searchWord));

        return query;
    }
}