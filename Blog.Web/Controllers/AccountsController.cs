using Blog.Services.ControllerContext;
using Blog.Services.Core.Utilities;
using Blog.Services.Identity.User;
using Blog.Web.VIewModels.AspNetUser;
using BLog.Web.ViewModels.Manage;

namespace Blog.Web.Controllers
{
    using Data.Models;
    using Services.Identity.Auth;
    using Services.Identity.Registration;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ViewModels.AspNetUser;

    /// <summary>
    /// Accounts controller.
    /// Registration and login user.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : BaseController
    {
        private readonly IUserService _userService;
        // private readonly IEmailExtensionService _emailExtensionService;
        // private readonly IMapper _mapper;
        private readonly IRegistrationService _registrationService;
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        // private readonly IRefreshTokenService _refreshTokenService;

        public AccountsController(
            IControllerContext controllerContext,
            IRegistrationService registrationService,
            IAuthService authService,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IUserService userService)
            : base(controllerContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService;
            // _emailExtensionService = emailService;
            // _mapper = mapper;
            _registrationService = registrationService;
            _authService = authService;
            // _refreshTokenService = refreshTokenService;
        }

        // GET: api/Users
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        /// <summary>
        /// Get user data by user id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>user json.</returns>
        [HttpGet("initialize/{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> InitializeAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return Bad("Something Went Wrong");
            }

            /*var notificationFilter = new StreamPageFilter
            {
                Length = 5,
                PageCount = 1
            };*/

            var jsonResult = new
            {
                roles = _roleManager.Roles,
            };
            return Ok(jsonResult);
        }

        // GET: api/Users/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// Get all users list.
        /// </summary>
        /// <returns>users.</returns>
        [HttpGet("get-all-users")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.Include(u => u.Roles).ToListAsync();
            return Ok(users);
        }

        /// <summary>
        /// User login.
        /// </summary>
        /// <param name="credentials">credentials.</param>
        /// <returns>jwt token.</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostAsync([FromBody] LoginViewModel credentials)
        {
            if (!ModelState.IsValid)
                return Bad(ModelState);

            var jwt = await _authService.GetJwtAsync(credentials.Email, credentials.Password);
            if (jwt == null)
                return Bad("Invalid name or password");
            var user = await _authService.GetByUserNameAsync(credentials.Email);
            if (user is null)
                return Bad("");

            return Ok(jwt);
        }

        /// <summary>
        /// Register user.
        /// </summary>
        /// <param name="model">model.</param>
        /// <returns>status.</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateAsync([FromBody] RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
                return Bad(ModelState);

            model.UserName = model.Email;
            // TODO Fix mapping  
            //var userIdentity = _mapper.Map<ApplicationUser>(model);
            var userIdentity = new ApplicationUser {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email,
                ConcurrencyStamp = model.ConcurrencyStamp,
                CreatedOn = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var result = await _registrationService.RegisterAsync(userIdentity, model.Password);

            if (!result.Succeeded)
                return Bad(result);


            /*
            //add all roles
            var role0 = new ApplicationRole { Name = "User", CreatedOn = DateTime.Now };
            await _roleManager.CreateAsync(role0);

            var role1 = new ApplicationRole { Name = "Moderator", CreatedOn = DateTime.Now };
            await _roleManager.CreateAsync(role1);
            var role2 = new ApplicationRole { Name = "Manager", CreatedOn = DateTime.Now };
            await _roleManager.CreateAsync(role2);
            var role3 = new ApplicationRole { Name = "Trainer", CreatedOn = DateTime.Now };
            await _roleManager.CreateAsync(role3);
            */

            if (model.Roles == null) model.Roles = new[] { "User" };
            
            foreach(var role in model.Roles)
            {
                var ir = await _userManager.AddToRoleAsync(userIdentity, role);
                if (!ir.Succeeded)
                    return BadRequest();
            }


            return NoContent();
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // PUT: api/Users/5
        [HttpPut("change-password")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateAsync([FromBody]ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return Bad(ModelState);

            var changePasswordResult = await _userService.ChangePasswordAsync(CurrentUser.UserName, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                ModelState.AddModelError(changePasswordResult.Errors);
                return Bad(ModelState);
            }
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}