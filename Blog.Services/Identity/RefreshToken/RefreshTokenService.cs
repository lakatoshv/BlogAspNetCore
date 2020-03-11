namespace Blog.Services.Identity.RefreshToken
{
    using System.Linq;
    using System.Threading.Tasks;
    using Blog.Data.Models;
    using Blog.Data.Repository;
    using Blog.Services.Core.Identity.Auth;
    using Blog.Services.Identity.Auth;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IRepository<Data.Models.RefreshToken> _refreshTokenRepository;
        private readonly IAuthService _authService;

        public RefreshTokenService(
            IJwtFactory jwtFactory,
            IOptions<JwtIssuerOptions> jwtOptions,
            IRepository<Data.Models.RefreshToken> refreshTokenRepository,
            IAuthService authService,
            UserManager<ApplicationUser> userManager
            )
        {
            _jwtOptions = jwtOptions.Value;
            _jwtFactory = jwtFactory;
            _refreshTokenRepository = refreshTokenRepository;
            _authService = authService;
            _userManager = userManager;
        }

        public async Task<string> RefreshTokensAsync(string userName, string refreshToken)
        {
            var user = await _userManager.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.UserName == userName);
            var identity = await _authService.GetClaimsIdentityWithoutPassword(userName);
            var token = _refreshTokenRepository.Table.Include(u => u.User).FirstOrDefault(t => t.Token.Equals(refreshToken));
            if (token == null)
            {
                return null;
            }
            _refreshTokenRepository.Delete(token);

            return await Tokens.GenerateJwt(identity, _jwtFactory, userName, _jwtOptions);

        }

        public async Task RemoveRefreshTokensAsync(string userName)
        {
            _refreshTokenRepository.Table.Include(r => r.User);
            var tokensToDelete = await _refreshTokenRepository.Table.Where(u => u.User.UserName == userName).ToListAsync();
            _refreshTokenRepository.Delete(tokensToDelete);

        }
    }
}
