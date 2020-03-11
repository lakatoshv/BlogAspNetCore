namespace Blog.Services.Core.Identity.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;
    using Blog.Data.Models;
    using Blog.Core;
    using System.IdentityModel.Tokens.Jwt;
    using Blog.Services.Core.Utilities;
    public class JwtClaimTypes
    {
        public const string Rol = "rol",
            Id = "id",
            UserName = "userName",
            Email = "email",
            PhoneNumber = "phoneNumber",
            IsEmailVerified = "isEmailVerified";
    }

    public class JwtClaims
    {
        public const string ApiAccess = "api_access";
    }

    public class JwtFactory : IJwtFactory
    {
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public JwtFactory(IOptions<JwtIssuerOptions> jwtOptions,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _jwtOptions = jwtOptions.Value;
            _userManager = userManager;
            _roleManager = roleManager;
            ThrowIfInvalidOptions(_jwtOptions);
        }

        public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity)
        {
            var claims = new List<Claim>(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, _jwtOptions.IssuedAt.ToUnixTimeStamp().ToString(),
                    ClaimValueTypes.Integer64),

                identity.FindFirst(JwtClaimTypes.Rol),
                identity.FindFirst(JwtClaimTypes.Id),
                identity.FindFirst(JwtClaimTypes.UserName),
                identity.FindFirst(JwtClaimTypes.Email),
                identity.FindFirst(JwtClaimTypes.PhoneNumber),
                identity.FindFirst(JwtClaimTypes.IsEmailVerified)
            });

            var user = await _userManager.FindByNameAsync(userName);

            claims.AddRange(await _userManager.GetClaimsAsync(user));

            var roleNames = await _userManager.GetRolesAsync(user);
            foreach (var roleName in roleNames)
            {
                // Find IdentityRole by name
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    // Convert Identity to claim and add 
                    var roleClaim = new Claim("roles", role.Name, ClaimValueTypes.String, _jwtOptions.Issuer);
                    claims.Add(roleClaim);

                    // Add claims belonging to the role
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    claims.AddRange(roleClaims);
                }
            }

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;
        }

        public ClaimsIdentity GenerateClaimsIdentity(ClaimsIdentityUserModel claimsIdentityUserModel)
        {
            return new ClaimsIdentity(new GenericIdentity(claimsIdentityUserModel.Email, "Token"), new[]
            {
                // TODO: Consider refactoring using nameof()
                new Claim(JwtClaimTypes.Id, claimsIdentityUserModel.Id),
                new Claim(JwtClaimTypes.Rol, JwtClaims.ApiAccess),
                new Claim(JwtClaimTypes.UserName, claimsIdentityUserModel.UserName),
                new Claim(JwtClaimTypes.Email, claimsIdentityUserModel.Email),
                new Claim(JwtClaimTypes.PhoneNumber, claimsIdentityUserModel.PhoneNumber ?? string.Empty),
                new Claim(JwtClaimTypes.IsEmailVerified, claimsIdentityUserModel.IsEmailVerified.ToString())
            });
        }

        public async Task<string> GenerateRefreshToken(string userName)
        {
            //change 
            var user = await _userManager.FindByNameAsync(userName);
            string refreshToken;
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken = Convert.ToBase64String(randomNumber);
            }
            user.RefreshTokens.Add(new Data.Models.RefreshToken { Token = refreshToken, User = user });

            await _userManager.UpdateAsync(user);
            return refreshToken;
        }

        #region Private

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));

            if (options.SigningCredentials == null)
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));

            if (options.JtiGenerator == null)
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
        }

        #endregion
    }
}
