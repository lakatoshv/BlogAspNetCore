// <copyright file="ApplicationUser.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Data.Models
{
    using System;
    using System.Collections.Generic;
    using Blog.Core;
    using Blog.Data.Core.Models.Interfaces;
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Application user entity.
    /// </summary>
    public class ApplicationUser : IdentityUser, IAuditInfo, IEntity<string>
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
        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        /// <summary>
        /// Gets or sets claims.
        /// </summary>
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        /// <summary>
        /// Gets or sets logins.
        /// </summary>
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        /// <summary>
        /// Gets or sets refresh tokens.
        /// </summary>
        private ICollection<RefreshToken> refreshTokens;

        // public ICollection<Post> Posts { get; set; }
        // public ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// Gets or sets refresh tokens.
        /// </summary>
        public virtual ICollection<RefreshToken> RefreshTokens
        {
            get => this.refreshTokens ?? (this.refreshTokens = new List<RefreshToken>());
            set => this.refreshTokens = value;
        }
    }
}
