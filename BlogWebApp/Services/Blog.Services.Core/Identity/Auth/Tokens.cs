// <copyright file="Tokens.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Identity.Auth
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    /// <summary>
    /// Tokens.
    /// </summary>
    public class Tokens
    {
        /// <summary>
        /// GenerateJwt.
        /// </summary>
        /// <param name="identity">identity.</param>
        /// <param name="jwtFactory">jwtFactory.</param>
        /// <param name="userName">userName.</param>
        /// <param name="jwtOptions">jwtOptions.</param>
        /// <returns>Task.</returns>
        public static async Task<string> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory, string userName, JwtIssuerOptions jwtOptions)
        {
            var serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };

            var response = new
            {
                id = identity.GetId(),
                auth_token = await jwtFactory.GenerateEncodedToken(userName, identity),
                refresh_token = await jwtFactory.GenerateRefreshToken(userName),
                expires_in = (int)jwtOptions.ValidFor.TotalSeconds,
            };

            var json = JsonConvert.SerializeObject(response, serializerSettings);
            return json;
        }
    }
}
