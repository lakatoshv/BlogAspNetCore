using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Services.Security.Authorization
{
    public class PermissionsRequirement : IAuthorizationRequirement
    {
        public string PermissionSystemName { get; }
        public PermissionsRequirement(string permissionSystemName) => PermissionSystemName = permissionSystemName;
    }
}
