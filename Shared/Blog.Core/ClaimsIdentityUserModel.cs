﻿// <copyright file="ClaimsIdentityUserModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core;

/// <summary>
/// Claims identity user model.
/// </summary>
public class ClaimsIdentityUserModel
{
    /// <summary>
    /// Gets or sets id.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets userName.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets email.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets phone number.
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether is email verified.
    /// </summary>
    public bool IsEmailVerified { get; set; }

    /// <summary>
    /// Gets or sets the profile identifier.
    /// </summary>
    /// <value>
    /// The profile identifier.
    /// </value>
    public int ProfileId { get; set; }
}