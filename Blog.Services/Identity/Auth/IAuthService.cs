namespace Blog.Services.Identity.Auth
{
    using Microsoft.AspNetCore.Identity;
    using Blog.Data.Models;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IAuthService
    {
        string GetJwt(string username, string password);
        Task<string> RefreshTokenAsync(string username);
        Task<string> GetJwtAsync(string username, string password);
        Task<ApplicationUser> GetByUserNameAsync(string username);
        Task<bool> VerifyTwoFactorTokenAsync(string username, string authenticatorCode);
        Task<IdentityResult> RedeemTwoFactorRecoveryCodeAsync(string username, string code);
        Task<ClaimsIdentity> GetClaimsIdentityWithoutPassword(string userName);
    }
}
