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
    /// Jwt claims.
    /// </summary>
    public class JwtClaims
    {
        /// <summary>
        /// Api access.
        /// </summary>
        public const string ApiAccess = "api_access";
    }
}
