// <copyright file="ApplicationRole.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Data.Models
{
    using System;
    using Blog.Data.Core.Models.Interfaces;
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Application role entity.
    /// </summary>
    /// <seealso cref="IdentityRole" />
    /// <seealso cref="IAuditInfo" />
    /// <seealso cref="IDeletableEntity" />
    public sealed class ApplicationRole : IdentityRole, IAuditInfo, IDeletableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRole"/> class.
        /// </summary>
        public ApplicationRole()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRole"/> class.
        /// </summary>
        /// <param name="name">ApplicationRole.</param>
        public ApplicationRole(string name)
            : base(name)
        {
            this.Id = Guid.NewGuid().ToString();
        }

        /// <inheritdoc cref="IAuditInfo"/>
        public DateTime CreatedOn { get; set; }

        /// <inheritdoc cref="IAuditInfo"/>
        public DateTime? ModifiedOn { get; set; }

        /// <inheritdoc cref="IDeletableEntity"/>
        public bool IsDeleted { get; set; }

        /// <inheritdoc cref="IDeletableEntity"/>
        public DateTime? DeletedOn { get; set; }
    }
}
