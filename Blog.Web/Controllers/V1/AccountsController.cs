using Blog.Services.ControllerContext;
using Blog.Services.Core.Utilities;
using Blog.Services.EmailServices.Interfaces;
using Blog.Services.Identity.User;
using Blog.Web.Contracts.V1;
using Blog.Web.VIewModels.AspNetUser;
using BLog.Web.ViewModels.Manage;

namespace Blog.Web.Controllers.V1
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
    [Route(ApiRoutes.AccountsController.Accounts)]
    [ApiController]
    [Authorize]
    public class AccountsController : BaseController
    {
        /// <summary>
        /// The user service.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// The email extension service.
        /// </summary>
        private readonly IEmailExtensionService _emailExtensionService;
        // private readonly IMapper _mapper;

        /// <summary>
        /// The registration service.
        /// </summary>
        private readonly IRegistrationService _registrationService;

        /// <summary>
        /// The authentication service.
        /// </summary>
        private readonly IAuthService _authService;

        /// <summary>
        /// The user manager.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// The role manager.
        /// </summary>
        private readonly RoleManager<ApplicationRole> _roleManager;
        // private readonly IRefreshTokenService _refreshTokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountsController"/> class.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="registrationService">The registration service.</param>
        /// <param name="authService">The authentication service.</param>
        /// <param name="userManager">The user manager.</param>
        /// <param name="roleManager">The role manager.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="emailService">The email service.</param>
        public AccountsController(
            IControllerContext controllerContext,
            IRegistrationService registrationService,
            IAuthService authService,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IUserService userService,
            IEmailExtensionService emailService)
            : base(controllerContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService;
            _emailExtensionService = emailService;
            // _mapper = mapper;
            _registrationService = registrationService;
            _authService = authService;
            // _refreshTokenService = refreshTokenService;
        }

        // GET: api/Users        
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet(ApiRoutes.AccountsController.Accounts)]
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
        [HttpGet(ApiRoutes.AccountsController.Initialize)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult Initialize(string userId)
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

        /// <summary>
        /// Get all users list.
        /// </summary>
        /// <returns>users.</returns>
        [HttpGet(ApiRoutes.AccountsController.GetAllUsers)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.Include(u => u.Roles).ToListAsync();
            return Ok(users);
        }

        /// <summary>
        /// Sends the verification email asynchronous.
        /// </summary>
        /// <returns></returns>
        [HttpGet(ApiRoutes.AccountsController.SendConfirmationEmail)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> SendVerificationEmailAsync()
        {
            var token = await _userService.GetEmailVerificationTokenAsync(CurrentUser.Email);
            await _emailExtensionService.SendVerificationEmailAsync(UserName, token);

            return NoContent();
        }

        /// <summary>
        /// User login.
        /// </summary>
        /// <param name="credentials">credentials.</param>
        /// <returns>jwt token.</returns>
        [HttpPost(ApiRoutes.AccountsController.Login)]
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
        [HttpPost(ApiRoutes.AccountsController.Register)]
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
        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut(ApiRoutes.AccountsController.ChangePassword)]
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
    }
}