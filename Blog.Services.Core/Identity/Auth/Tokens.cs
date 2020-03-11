namespace Blog.Services.Core.Identity.Auth
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class Tokens
    {
        public static async Task<string> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory, string userName, JwtIssuerOptions jwtOptions)
        {
            var serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };

            var response = new
            {
                id = identity.GetId(),
                auth_token = await jwtFactory.GenerateEncodedToken(userName, identity),
                refresh_token = await jwtFactory.GenerateRefreshToken(userName),
                expires_in = (int)jwtOptions.ValidFor.TotalSeconds
            };

            var json = JsonConvert.SerializeObject(response, serializerSettings);
            return json;
        }
    }
}
