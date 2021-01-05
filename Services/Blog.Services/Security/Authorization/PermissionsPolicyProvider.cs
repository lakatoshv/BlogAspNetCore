using System;
using System.Threading.Tasks;
using Blog.Core.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Blog.Services.Security.Authorization
{
    public class PermissionsPolicyProvider : IAuthorizationPolicyProvider
    {
        string policyPrefix = Consts.PolicyPrefix;
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public PermissionsPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (!policyName.StartsWith(policyPrefix, StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrWhiteSpace(policyName.Substring(policyPrefix.Length)))
            {
                return FallbackPolicyProvider.GetPolicyAsync(policyName);
            }

            var policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new PermissionsRequirement(policyName.Substring(policyPrefix.Length)));
            return Task.FromResult(policy.Build());
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();
    }
}
