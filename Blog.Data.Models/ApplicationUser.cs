﻿namespace Blog.Data.Models
{
    using System;
    using System.Collections.Generic;
    using Blog.Core;
    using Blog.Data.Core.Models.Interfaces;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IEntity<string>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
        private ICollection<RefreshToken> _refreshTokens;
        //public ICollection<Post> Posts { get; set; }
        //public ICollection<Comment> Comments { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens
        {
            get => _refreshTokens ?? (_refreshTokens = new List<RefreshToken>());
            set => _refreshTokens = value;
        }
    }
}
