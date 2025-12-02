// <copyright file="HasScopeRequirement.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Security;

using Microsoft.AspNetCore.Authorization;
using System;

/// <summary>
/// Has scope requirement.
/// </summary>
public class HasScopeRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HasScopeRequirement"/> class.
    /// </summary>
    /// <param name="scope">scope.</param>
    /// <param name="issuer">issuer.</param>
    public HasScopeRequirement(string scope, string issuer)
    {
        this.Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        this.Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
    }

    /// <summary>
    /// Gets issuer.
    /// </summary>
    public string Issuer { get; }

    /// <summary>
    /// Gets scope.
    /// </summary>
    public string Scope { get; }
}