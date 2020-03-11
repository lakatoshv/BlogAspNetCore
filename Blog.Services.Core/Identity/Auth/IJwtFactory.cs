namespace Blog.Services.Core.Identity.Auth
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Blog.Core;

    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);
        Task<string> GenerateRefreshToken(string userName);
        ClaimsIdentity GenerateClaimsIdentity(ClaimsIdentityUserModel claimsIdentityUserModel);
    }
}
