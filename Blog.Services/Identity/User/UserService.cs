// <copyright file="UserService.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Services.Identity.User
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Blog.Core.Infrastructure.Pagination;
    using Blog.Core.TableFilters;
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
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Repository for ApplicationUser.
        /// </summary>
        private readonly IRepository<ApplicationUser> _applicationUserRepository;

        /// <summary>
        /// Mapper.
        /// </summary>
        private readonly IMapper _mapper;

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
            this._userManager = userManager;
            this._applicationUserRepository = applicationUserRepository;
            this._mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            var result = await this._userManager.CreateAsync(user, password);
            return result;
        }

        /// <inheritdoc/>
        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            var user = await this._userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        /// <inheritdoc/>
        public async Task<IList<ApplicationUser>> GetUsersAsync(IEnumerable<string> userIds)
        {
            return await this._userManager.Users
                .Where(x => userIds.Contains(x.Id))
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ApplicationUser> GetByEmailAsync(string email)
        {
            return await this._userManager.Users
                .FirstOrDefaultAsync(m => m.Email == email);
        }

        /// <inheritdoc/>
        public async Task<IdentityResult> ChangePasswordAsync(string userName, string oldPassword, string newPassword)
        {
            var user = await this.GetByUserNameAsync(userName);
            var changePasswordResult = await this._userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return changePasswordResult;
        }

        /// <inheritdoc/>
        public async Task<List<ApplicationUser>> GetAllAsync()
        {
            var userList = await this._userManager.Users.ToListAsync();
            return userList;
        }

        /// <inheritdoc/>
        public async Task<string> GetAuthenticatorKeyAsync(string userName)
        {
            var user = await this.GetByUserNameAsync(userName);
            var token = await this._userManager.GetAuthenticatorKeyAsync(user);

            return token;
        }

        /// <inheritdoc/>
        public Task<string> GetAuthenticatorKeyAsync(ApplicationUser user)
        {
            return this._userManager.GetAuthenticatorKeyAsync(user);
        }

        /// <inheritdoc/>
        public async Task<IdentityResult> ResetAuthenticatorKeyAsync(ApplicationUser user)
        {
            return await this._userManager.ResetAuthenticatorKeyAsync(user);
        }

        /// <inheritdoc/>
        public async Task<ApplicationUser> GetByUserNameAsync(string userName)
        {
            return await this._userManager.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        /// <inheritdoc/>
        public async Task<ApplicationUser> GetByProfileIdAsync(int profileId)
        {
            return
                await this._userManager.Users
                    .FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task DelByIdAsync(string id)
        {
            var user = await this._userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                user.Email = user.Id;
                user.UserName = user.Id;

                await this._userManager.UpdateAsync(user);
            }
        }

        /// <inheritdoc/>
        public async Task<string> GetEmailVerificationTokenAsync(string userName)
        {
            var user = await this._userManager.FindByNameAsync(userName);
            var token = await this._userManager.GenerateEmailConfirmationTokenAsync(user);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(token);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <inheritdoc/>
        public async Task<string> GetPasswordResetTokenAsync(string userName)
        {
            var user = await this._userManager.FindByNameAsync(userName);
            var token = await this._userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }

        /// <inheritdoc/>
        public async Task<IdentityResult> UpdateAsync(string userName, ApplicationUser applicationUser)
        {
            var user = await this._userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            this._mapper.Map(applicationUser, user);

            var result = await this._userManager.UpdateAsync(user);
            return result;
        }

        /// <inheritdoc/>
        public async Task<IdentityResult> VerifyEmailAsync(string userName, string token)
        {
            var user = await this._userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            var base64EncodedBytes = System.Convert.FromBase64String(token);
            token = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

            var result = await this._userManager.ConfirmEmailAsync(user, token);
            return result;
        }

        /// <inheritdoc/>
        public Task<int> CountRecoveryCodesAsync(ApplicationUser user)
        {
            return this._userManager.CountRecoveryCodesAsync(user);
        }

        /// <inheritdoc/>
        public Task<bool> VerifyTwoFactorTokenAsync(ApplicationUser user, string tokenProvider, string token)
        {
            return this._userManager.VerifyTwoFactorTokenAsync(user, tokenProvider, token);
        }

        /// <inheritdoc/>
        public string GetAuthenticationProvider()
        {
            return this._userManager.Options.Tokens.AuthenticatorTokenProvider;
        }

        /// <inheritdoc/>
        public Task<IdentityResult> SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            return this._userManager.SetTwoFactorEnabledAsync(user, enabled);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<string>> GenerateNewTwoFactorRecoveryCodesAsync(ApplicationUser user, int number)
        {
            return this._userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, number);
        }

        /// <inheritdoc/>
        public async Task<IdentityResult> ResetPasswordAsync(string userName, string token, string newPassword)
        {
            var user = await this.GetByUserNameAsync(userName);
            var result = await this._userManager.ResetPasswordAsync(user, token, newPassword);
            return result;
        }

        /// <inheritdoc/>
        public async Task<PagedListResult<ApplicationUser>> GetAllFilteredUsersAsync(TableFilter tableFilter)
        {
            var sequence = this._applicationUserRepository.Table;

            return await this._applicationUserRepository.SearchBySquenceAsync(this.AddFilter(tableFilter), sequence);
        }

        /// <summary>
        /// Add filter.
        /// </summary>
        /// <param name="tableFilter">tableFilter.</param>
        /// <returns>SearchQuery.</returns>
        private SearchQuery<ApplicationUser> AddFilter(TableFilter tableFilter)
        {
            var query = this._applicationUserRepository.GenerateQuery(tableFilter);
            var searchWord = tableFilter.Search.Value.ToUpper();

            query.AddFilter(x =>
                                 x.UserName.ToUpper().Contains(searchWord)
                                  || x.Email.ToUpper().Contains(searchWord)
                                  || x.UserName.ToUpper().Contains(searchWord)
                                  || x.PhoneNumber.Contains(searchWord));

            return query;
        }
    }
}
