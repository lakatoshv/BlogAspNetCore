﻿// <copyright file="BlogAuthorizeAttribute.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Security.Authorization
{
    using Blog.Core.Consts;
    using Microsoft.AspNetCore.Authorization;

    /// <summary>
    /// Blog authorize attribute.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Authorization.AuthorizeAttribute" />
    public class BlogAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// The policy prefix.
        /// </summary>
        private string policyPrefix = Consts.PolicyPrefix;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="systemName">Name of the system.</param>
        public BlogAuthorizeAttribute(string systemName) => this.PermissionSystemName = systemName;

        /// <summary>
        /// Gets or sets the name of the permission system.
        /// </summary>
        /// <value>
        /// The name of the permission system.
        /// </value>
        public string PermissionSystemName
        {
            get => this.Policy.Substring(this.policyPrefix.Length);
            set => this.Policy = $"{this.policyPrefix}{value}";
        }
    }
}
