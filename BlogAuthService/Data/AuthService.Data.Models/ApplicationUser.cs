namespace AuthService.Data.Models
{
    using System;
    using System.Collections.Generic;
    using AuthService.Core;
    using AuthService.Data.Core.Models.Interfaces;
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Application user.
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
        /// <remarks>
        /// The Id property is initialized to form a new GUID string value.
        /// </remarks>
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
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public ICollection<IdentityUserRole<string>> Roles { get; set; }

        /// <summary>
        /// Gets or sets the claims.
        /// </summary>
        /// <value>
        /// The claims.
        /// </value>
        public ICollection<IdentityUserClaim<string>> Claims { get; set; }

        /// <summary>
        /// Gets or sets the logins.
        /// </summary>
        /// <value>
        /// The logins.
        /// </value>
        public ICollection<IdentityUserLogin<string>> Logins { get; set; }

        /// <summary>
        /// The refresh tokens.
        /// </summary>
        private ICollection<RefreshToken> _refreshTokens;

        // public ICollection<Post> Posts { get; set; }
        // public ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// Gets or sets the refresh tokens.
        /// </summary>
        /// <value>
        /// The refresh tokens.
        /// </value>
        public ICollection<RefreshToken> RefreshTokens
        {
            get => _refreshTokens ?? (_refreshTokens = new List<RefreshToken>());
            set => _refreshTokens = value;
        }
    }
}
