// <copyright file="ApplicationUser.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Data.Models;

using System;
using System.Collections.Generic;
using Blog.Core;
using Blog.Data.Core.Models.Interfaces;
using Microsoft.AspNetCore.Identity;

/// <summary>
/// Application user entity.
/// </summary>
/// <seealso cref="IdentityUser" />
/// <seealso cref="IAuditInfo" />
/// <seealso>
///     <cref>IEntity{string}</cref>
/// </seealso>
public sealed class ApplicationUser : IdentityUser, IAuditInfo, IEntity<string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationUser"/> class.
    /// </summary>
    public ApplicationUser()
    {
        this.Id = Guid.NewGuid().ToString();
        this.Roles = new HashSet<IdentityUserRole<string>>();
        this.Claims = new HashSet<IdentityUserClaim<string>>();
        this.Logins = new HashSet<IdentityUserLogin<string>>();
    }

    /// <summary>
    /// Gets or sets first name.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets last name.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets created on.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets modified on.
    /// </summary>
    public DateTime? ModifiedOn { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether is deleted.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets deleted on.
    /// </summary>
    public DateTime? DeletedOn { get; set; }

    /// <summary>
    /// Gets or sets roles.
    /// </summary>
    public ICollection<IdentityUserRole<string>> Roles { get; set; }

    /// <summary>
    /// Gets or sets claims.
    /// </summary>
    public ICollection<IdentityUserClaim<string>> Claims { get; set; }

    /// <summary>
    /// Gets or sets logins.
    /// </summary>
    public ICollection<IdentityUserLogin<string>> Logins { get; set; }

    /// <summary>
    /// Gets or sets refresh tokens.
    /// </summary>
    private ICollection<RefreshToken> _refreshTokens;

    // public ICollection<Post> Posts { get; set; }
    // public ICollection<Comment> Comments { get; set; }

    /// <summary>
    /// Gets or sets refresh tokens.
    /// </summary>
    public ICollection<RefreshToken> RefreshTokens
    {
        get => this._refreshTokens ?? (this._refreshTokens = new List<RefreshToken>());
        set => this._refreshTokens = value;
    }

    /// <summary>
    /// Gets or sets the profile.
    /// </summary>
    /// <value>
    /// The profile.
    /// </value>
    public Profile Profile { get; set; }

    /// <summary>
    /// Gets or sets the posts.
    /// </summary>
    /// <value>
    /// The posts.
    /// </value>
    public ICollection<Post> Posts { get; set; }


    /// <summary>
    /// Gets or sets the comments.
    /// </summary>
    /// <value>
    /// The comments.
    /// </value>
    public ICollection<Comment> Comments { get; set; }

    /// <summary>
    /// Gets or sets the received messages.
    /// </summary>
    /// <value>
    /// The received messages.
    /// </value>
    public ICollection<Message> ReceivedMessages { get; set; }

    /// <summary>
    /// Gets or sets the sent messages.
    /// </summary>
    /// <value>
    /// The sent messages.
    /// </value>
    public ICollection<Message> SentMessages { get; set; }
}