// <copyright file="PermissionsPolicyProvider.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.EntityServices.EntityFrameworkServices.Security.Authorization;

using Core.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

/// <summary>
/// Permissions policy provider.
/// </summary>
/// <seealso cref="IAuthorizationPolicyProvider" />
/// <remarks>
/// Initializes a new instance of the <see cref="PermissionsPolicyProvider"/> class.
/// </remarks>
/// <param name="options">The options.</param>
public class PermissionsPolicyProvider(IOptions<AuthorizationOptions> options)
    : IAuthorizationPolicyProvider
{
    /// <summary>
    /// The policy prefix.
    /// </summary>
    private string policyPrefix = Consts.PolicyPrefix;

    /// <summary>
    /// Gets the fallback policy provider.
    /// </summary>
    /// <value>
    /// The fallback policy provider.
    /// </value>
    public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; } = new(options);

    /// <summary>
    /// Gets a <see cref="T:Microsoft.AspNetCore.Authorization.AuthorizationPolicy" /> from the given <paramref name="policyName" />
    /// </summary>
    /// <param name="policyName">The policy name to retrieve.</param>
    /// <returns>
    /// The named <see cref="T:Microsoft.AspNetCore.Authorization.AuthorizationPolicy" />.
    /// </returns>
    public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        if (!policyName.StartsWith(this.policyPrefix, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrWhiteSpace(policyName[this.policyPrefix.Length..]))
        {
            return this.FallbackPolicyProvider.GetPolicyAsync(policyName);
        }

        var policy = new AuthorizationPolicyBuilder();
        policy.AddRequirements(new PermissionsRequirement(policyName.Substring(this.policyPrefix.Length)));
        return Task.FromResult(policy.Build());
    }

    /// <summary>
    /// Gets the default authorization policy.
    /// </summary>
    /// <returns>
    /// The default authorization policy.
    /// </returns>
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        => this.FallbackPolicyProvider.GetDefaultPolicyAsync();

    /// <summary>
    /// Gets the fallback authorization policy.
    /// </summary>
    /// <returns>
    /// The fallback authorization policy.
    /// </returns>
    public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        => ((IAuthorizationPolicyProvider)this.FallbackPolicyProvider).GetFallbackPolicyAsync();
}