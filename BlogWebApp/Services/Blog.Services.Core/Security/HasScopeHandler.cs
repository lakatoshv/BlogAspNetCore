// <copyright file="HasScopeHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Security;

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

/// <summary>
/// Has scope handler.
/// </summary>
public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
{
    /// <inheritdoc cref="AuthorizationHandler{HasScopeRequirement}"/>
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
    {
        // If user does not have the scope claim, get out of here
        if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == requirement.Issuer))
        {
            return Task.CompletedTask;
        }

        // Split the scopes string into an array
        var scopes = context.User.FindFirst(c => c.Type == "scope" && c.Issuer == requirement.Issuer).Value.Split(' ');

        // Succeed if the scope array contains the required scope
        if (scopes.Any(s => s == requirement.Scope))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}