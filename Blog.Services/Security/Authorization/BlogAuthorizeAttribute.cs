using Blog.Core.Consts;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Services.Security.Authorization
{
    public class BlogAuthorizeAttribute : AuthorizeAttribute
    {
        string policyPrefix = Consts.PolicyPrefix;
        public BlogAuthorizeAttribute(string systemName) => PermissionSystemName = systemName;

        public string PermissionSystemName
        {
            get => Policy.Substring(policyPrefix.Length);
            set => Policy = $"{policyPrefix}{value}";
        }
    }
}
