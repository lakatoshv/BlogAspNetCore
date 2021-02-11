// <copyright file="MyControllerContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.ControllerContext
{
    using System.Security.Claims;
    using Blog.Data.Models;
    using Blog.Services.Identity.User;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controller context.
    /// </summary>
    public class MyControllerContext : ControllerBase, IControllerContext
    {
        /// <summary>
        /// Http context accessor.
        /// </summary>
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// User service.
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        /// Application user.
        /// </summary>
        private ApplicationUser cachedUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyControllerContext"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">httpContextAccessor.</param>
        /// <param name="userService">userService.</param>
        public MyControllerContext(IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userService = userService;
        }

        /// <inheritdoc cref="IControllerContext"/>
        public ApplicationUser CurrentUser
        {
            get
            {
                // whether there is a cached value
                if (this.cachedUser != null)
                {
                    return this.cachedUser;
                }

                var httpContextUser = this.httpContextAccessor.HttpContext.User;
                var userName = httpContextUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userName == null)
                {
                    return null;
                }

                var user = this.userService?.GetByUserNameAsync(userName).Result;

                if (user != null)
                {
                    this.cachedUser = user;
                }

                return this.cachedUser;
            }
            set => this.cachedUser = value;
        }
    }
}
