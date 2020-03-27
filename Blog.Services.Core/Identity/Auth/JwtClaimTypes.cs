// <copyright file="JwtClaimTypes.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Identity.Auth
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using Blog.Core;
    using Blog.Data.Models;
    using Blog.Services.Core.Utilities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Jwt claim types.
    /// </summary>
    public class JwtClaimTypes
    {
        /// <summary>
        /// Rol.
        /// </summary>
        public const string Rol = "rol";

        /// <summary>
        /// Id.
        /// </summary>
        public const string Id = "id";

        /// <summary>
        /// UserName.
        /// </summary>
        public const string UserName = "userName";

        /// <summary>
        /// Email.
        /// </summary>
        public const string Email = "email";

        /// <summary>
        /// PhoneNumber.
        /// </summary>
        public const string PhoneNumber = "phoneNumber";

        /// <summary>
        /// IsEmailVerified.
        /// </summary>
        public const string IsEmailVerified = "isEmailVerified";
    }
}
