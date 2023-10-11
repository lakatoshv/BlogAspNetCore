// <copyright file="MyControllerContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.EntityServices.ControllerContext;

using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Data.Models;
using Identity.User;

/// <summary>
/// Controller context.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MyControllerContext"/> class.
/// </remarks>
/// <param name="httpContextAccessor">httpContextAccessor.</param>
/// <param name="userService">userService.</param>
public class MyControllerContext(IHttpContextAccessor httpContextAccessor, IUserService userService)
    : ControllerBase, IControllerContext
{
    /// <summary>
    /// Http context accessor.
    /// </summary>
    private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;

    /// <summary>
    /// User service.
    /// </summary>
    private readonly IUserService userService = userService;

    /// <summary>
    /// Application user.
    /// </summary>
    private ApplicationUser cachedUser;

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

            var httpContextUser = this.httpContextAccessor?.HttpContext?.User;
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