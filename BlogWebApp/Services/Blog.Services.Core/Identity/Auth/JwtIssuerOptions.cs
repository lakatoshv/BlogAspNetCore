// <copyright file="JwtIssuerOptions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Identity.Auth;

using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

/// <summary>
/// Jwt issuer options.
/// </summary>
public class JwtIssuerOptions
{
    /// <summary>
    /// Gets or sets issuer.
    /// </summary>
    public string Issuer { get; set; }

    /// <summary>
    /// Gets or sets subject.
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Gets or sets audience.
    /// </summary>
    public string Audience { get; set; }

    /// <summary>
    /// Gets expiration.
    /// </summary>
    public DateTime Expiration => this.IssuedAt.Add(this.ValidFor);

    /// <summary>
    /// Gets notBefore.
    /// </summary>
    public DateTime NotBefore => DateTime.UtcNow;

    /// <summary>
    /// Gets issuedAt.
    /// </summary>
    public DateTime IssuedAt => DateTime.UtcNow;

    /// <summary>
    /// Gets or sets validFor.
    /// </summary>
    public TimeSpan ValidFor { get; set; } = TimeSpan.FromHours(2);

    /// <summary>
    /// Gets jtiGenerator.
    /// </summary>
    public Func<Task<string>> JtiGenerator => () => Task.FromResult(Guid.NewGuid().ToString());

    /// <summary>
    /// Gets or sets signingCredentials.
    /// </summary>
    public SigningCredentials SigningCredentials { get; set; }
}