// <copyright file="Profile.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Data.Models;

using Core;

/// <summary>
/// Profile table.
/// </summary>
/// <seealso cref="Entity" />
public class Profile : Entity
{
    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    /// <value>
    /// The user identifier.
    /// </value>
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the user.
    /// </summary>
    /// <value>
    /// The user.
    /// </value>
    public virtual ApplicationUser User { get; set; }

    /// <summary>
    /// Gets or sets the About.
    /// </summary>
    /// <value>
    /// The About.
    /// </value>
    public string About { get; set; }

    /// <summary>
    /// Gets or sets the profile img.
    /// </summary>
    /// <value>
    /// The profile img.
    /// </value>
    public string ProfileImg { get; set; }
}