using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blog.Services.Security.Authorization
{
    public class PermissionsHandler : AuthorizationHandler<PermissionsRequirement>
    {
        private readonly IPermissionService _permissionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionsHandler(IPermissionService permissionService, IHttpContextAccessor httpContextAccessor)
        {
            _permissionService = permissionService;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionsRequirement requirement)
        {
            var hasPermissions = _permissionService.Authorize();
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
