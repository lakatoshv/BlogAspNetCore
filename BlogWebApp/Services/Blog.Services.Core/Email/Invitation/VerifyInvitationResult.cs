// <copyright file="VerifyInvitationResult.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Email.Invitation;

/// <summary>
/// Verify invitation result.
/// </summary>
public class VerifyInvitationResult
{
    /// <summary>
    /// Gets or sets inviterEmail.
    /// </summary>
    public string InviterEmail { get; set; }

    /// <summary>
    /// Gets or sets securityStamp.
    /// </summary>
    public string SecurityStamp { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether isVerified.
    /// </summary>
    public bool IsVerified { get; set; }
}