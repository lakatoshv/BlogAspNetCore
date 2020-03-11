namespace Blog.Web.Core.ControllerContext
{
    using Blog.Data.Models;
    using Blog.Services.Identity.User;
    using Microsoft.AspNetCore.Http;
    using System.Security.Claims;

    public class MyControllerContext : IControllerContext
    {
        private ApplicationUser _cacheduser;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        public MyControllerContext(IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        public ApplicationUser CurrentUser
        {
            get
            {
                //whether there is a cached value
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
