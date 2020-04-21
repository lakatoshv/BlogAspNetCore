using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Services.ControllerContext
{
    using System.Security.Claims;
    using Data.Models;
    using Identity.User;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Controller context.
    /// </summary>
    public class MyControllerContext : ControllerBase, IControllerContext
    {
        /// <summary>
        /// Http context accessor.
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// User service.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Application user.
        /// </summary>
        private ApplicationUser _cachedUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyControllerContext"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">httpContextAccessor.</param>
        /// <param name="userService">userService.</param>
        public MyControllerContext(IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._userService = userService;
        }

        /// <inheritdoc/>
        public ApplicationUser CurrentUser
        {
            get
            {
                // whether there is a cached value
                if (this._cachedUser != null)
                {
                    return this._cachedUser;
                }

                var httpContextUser = this._httpContextAccessor.HttpContext.User;
                var userName = httpContextUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userName == null)
                {
                    return null;
                }

                var user = this._userService?.GetByUserNameAsync(userName).Result;

                if (user != null)
                {
                    this._cachedUser = user;
                }

                return this._cachedUser;
            }
            set => this._cachedUser = value;
        }
    }
}
