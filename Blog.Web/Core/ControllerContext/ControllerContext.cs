namespace Blog.Web.Core.ControllerContext
{
    using Data.Models;
    using Services.Identity.User;
    using Microsoft.AspNetCore.Http;
    using System.Security.Claims;

    /// <summary>
    /// Controller context.
    /// </summary>
    public class MyControllerContext : IControllerContext
    {
        /// <summary>
        /// Application user.
        /// </summary>
        private ApplicationUser _cacheduser;

        /// <summary>
        /// Http context accessor.
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// User service
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes static members of the <see cref="ControllerContext"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">httpContextAccessor.</param>
        /// <param name="userService">userService.</param>
        public MyControllerContext(IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        /// <inheritdoc/>
        public ApplicationUser CurrentUser
        {
            get
            {
                // whether there is a cached value
                if (_cacheduser != null)
                    return _cacheduser;

                var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var user = _userService.GetByUserNameAsync(userName).Result;

                if (user != null)
                {
                    _cacheduser = user;
                }

                return _cacheduser;
            }
            set
            {
                _cacheduser = value;
            }
        }
    }
}
