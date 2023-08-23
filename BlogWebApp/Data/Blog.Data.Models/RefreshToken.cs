// <copyright file="RefreshToken.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Data.Models;

using Core;

/// <summary>
/// Refresh token.
/// </summary>
public class RefreshToken : Entity
{
    /// <summary>
    /// Gets or sets token.
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// Gets or sets user.
    /// </summary>
    public virtual ApplicationUser User { get; set; }

    /// <summary>
    /// Gets or sets user id.
    /// </summary>
    public string UserId { get; set; }
}