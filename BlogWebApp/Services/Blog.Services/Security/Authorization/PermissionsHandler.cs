// <copyright file="PermissionsHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Security.Authorization
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// Permissions handler.
    /// </summary>
    /// <seealso cref="AuthorizationHandler{PermissionsRequirement}" />
    public class PermissionsHandler : AuthorizationHandler<PermissionsRequirement>
    {
        /// <summary>
        /// The permission service.
        /// </summary>
        private readonly IPermissionService permissionService;

        /// <summary>
        /// The HTTP context accessor.
        /// </summary>
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionsHandler"/> class.
        /// </summary>
        /// <param name="permissionService">The permission service.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public PermissionsHandler(IPermissionService permissionService, IHttpContextAccessor httpContextAccessor)
        {
            this.permissionService = permissionService;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Makes a decision if authorization is allowed based on a specific requirement.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The requirement to evaluate.</param>
        /// <returns>Task.</returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionsRequirement requirement)
        {
            var hasPermissions = this.permissionService.Authorize();
            if (hasPermissions)
            {
                context.Succeed(requirement);
            }
            else
            {
                if (context.Resource is AuthorizationFilterContext mvcContext)
                {
                    mvcContext.Result = new JsonResult("You do not have permission to do this action") { StatusCode = 403 };
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
