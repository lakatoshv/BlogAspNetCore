namespace Blog.Services.Core.Identity.Auth
{
    using System.Linq;
    using System.Security.Claims;

    internal static class Extensions
    {
        public static string GetId(this ClaimsIdentity identity)
        {
            var id = identity.Claims.Single(c => c.Type == JwtClaimTypes.Id)?.Value;

            return id;
        }
    }
}
