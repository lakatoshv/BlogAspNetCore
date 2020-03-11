namespace Blog.Services.Identity.RefreshToken
{
    using System.Threading.Tasks;

    public interface IRefreshTokenService
    {
        Task<string> RefreshTokensAsync(string userName, string refreshToken);
        Task RemoveRefreshTokensAsync(string userName);
    }
}
