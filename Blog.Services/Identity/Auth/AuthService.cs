namespace Blog.Services.Identity.Auth
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Blog.Core;
    using Blog.Data.Models;
    using Blog.Services.Core.Identity.Auth;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    public class AuthService : IAuthService
    {
        // TODO: Add another abstraction level against UserManager
        // so it can be replaced with non identity implementation
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;

        public AuthService(UserManager<ApplicationUser> userManager,
            IJwtFactory jwtFactory,
            IOptions<JwtIssuerOptions> jwtOptions
            )
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
        }

        public string GetJwt(string username, string password)
        {
            // TODO: Either implement or remove
            throw new NotImplementedException();
        }
        public async Task<ApplicationUser> GetByUserNameAsync(string username)
        {
            return await _userManager.Users
                .Where(x => x.Email.Equals(username))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> VerifyTwoFactorTokenAsync(string username, string authenticatorCode)
        {
            var user = await GetByUserNameAsync(username);
            return await _userManager.VerifyTwoFactorTokenAsync(
               user, _userManager.Options.Tokens.AuthenticatorTokenProvider, authenticatorCode);
        }
        public async Task<IdentityResult> RedeemTwoFactorRecoveryCodeAsync(string username, string code)
        {
            var user = await GetByUserNameAsync(username);
            return await _userManager.RedeemTwoFactorRecoveryCodeAsync(user, code);
        }
        public async Task<string> GetJwtAsync(string username, string password)
        {
            var identity = await GetClaimsIdentity(username, password);
            if (identity == null)
                return null;

            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, username, _jwtOptions);
            return jwt;
        }

        public async Task<string> RefreshTokenAsync(string username)
        {
            var identity = await GetClaimsIdentityWithoutPassword(username);
            if (identity == null)
                return null;

            return await Tokens.GenerateJwt(identity, _jwtFactory, username, _jwtOptions);
        }



        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);

            if (userToVerify == null)
                return await Task.FromResult<ClaimsIdentity>(null);

            var claimsIdentityUserModel = GetIdentityClaims(userToVerify, userName);

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(claimsIdentityUserModel));

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        public async Task<ClaimsIdentity> GetClaimsIdentityWithoutPassword(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);

            if (userToVerify == null)
                return await Task.FromResult<ClaimsIdentity>(null);

            var claimsIdentityUserModel = GetIdentityClaims(userToVerify, userName);

            return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(claimsIdentityUserModel));
        }

        private ClaimsIdentityUserModel GetIdentityClaims(ApplicationUser userToVerify, string userName)
        {
            return new ClaimsIdentityUserModel
            {
                Id = userToVerify.Id,
                Email = userName,
                UserName = userToVerify.UserName,
                PhoneNumber = userToVerify.PhoneNumber,
                IsEmailVerified = userToVerify.EmailConfirmed,
            };
        }
    }
}
