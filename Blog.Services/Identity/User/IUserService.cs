namespace Blog.Services.Identity.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Blog.Core.Infrastructure.Pagination;
    using Blog.Core.TableFilters;
    using Blog.Data.Models;
    using Microsoft.AspNetCore.Identity;

    public interface IUserService
    {

        Task<PagedListResult<ApplicationUser>> GetAllFilteredUsersAsync(TableFilter tableFilter);
        Task<List<ApplicationUser>> GetAllAsync();
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
        Task DelByIdAsync(string id);
        Task<ApplicationUser> GetByUserNameAsync(string userName);
        Task<ApplicationUser> GetByProfileIdAsync(int profileId);
        Task<ApplicationUser> GetByEmailAsync(string email);
        Task<ApplicationUser> GetByIdAsync(string id);
        Task<IList<ApplicationUser>> GetUsersAsync(IEnumerable<string> userIds);

        Task<IdentityResult> ChangePasswordAsync(string userName, string oldPassword, string newPassword);
        Task<string> GetEmailVerificationTokenAsync(string userName);
        Task<string> GetPasswordResetTokenAsync(string userName);
        Task<string> GetAuthenticatorKeyAsync(string userName);
        Task<string> GetAuthenticatorKeyAsync(ApplicationUser user);

        Task<IdentityResult> ResetAuthenticatorKeyAsync(ApplicationUser user);
        Task<int> CountRecoveryCodesAsync(ApplicationUser user);
        Task<bool> VerifyTwoFactorTokenAsync(ApplicationUser user, string tokenProvider, string token);
        string GetAuthenticationProvider();
        Task<IdentityResult> SetTwoFactorEnabledAsync(ApplicationUser user, bool allow);
        Task<IEnumerable<string>> GenerateNewTwoFactorRecoveryCodesAsync(ApplicationUser user, int number);

        Task<IdentityResult> UpdateAsync(string userName, ApplicationUser applicationUser);
        Task<IdentityResult> VerifyEmailAsync(string userName, string token);
        Task<IdentityResult> ResetPasswordAsync(string userName, string token, string newPassword);
    }
}
