using Blog.Data.Models;
using Blog.Services.ControllerContext;
using Blog.Services.Core.Caching;
using Blog.Services.Core.Caching.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Blog.Services.Security
{
    /// <summary>
    /// Permission service
    /// </summary>
    public class PermissionService : IPermissionService
    {

        private readonly IControllerContext _workContext;
        private readonly IStaticCacheManager _cacheManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public PermissionService(IControllerContext workContext, IStaticCacheManager cacheManager,
            UserManager<ApplicationUser> userManager)
        {
            _workContext = workContext;
            _cacheManager = cacheManager;
            _userManager = userManager;
        }

        /// <inheritdoc />
        public bool Authorize()
        {
            return Authorize(_workContext.CurrentUser);
        }

        private bool Authorize(ApplicationUser user)
        {
            if (CheckIsUserAdmin(user)) return true;
            return true;
        }

        private bool CheckIsUserAdmin(ApplicationUser user)
        {
            string systemAdminKey = string.Format("Admin", user.Id);
            return _cacheManager.Get(systemAdminKey, () => _userManager.IsInRoleAsync(user, "Admin").Result);
        }
    }
}
