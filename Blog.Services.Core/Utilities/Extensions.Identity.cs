namespace Blog.Services.Core.Utilities
{
    using System.Security.Claims;

    public static partial class Extensions
    {
        public static string GetUserName(this ClaimsPrincipal identity)
        {
            var username = identity.FindFirst(ClaimTypes.NameIdentifier);

            return username?.Value;
        }
    }
}
