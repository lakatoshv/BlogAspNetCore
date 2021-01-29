﻿using System.Collections.Generic;
using AutoMapper;
using Blog.Services.ControllerContext;
using Blog.Services.Core.Utilities;
using Blog.Services.EmailServices.Interfaces;
using Blog.Services.Identity.User;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Requests.UsersRequests;
using Blog.Contracts.V1.Responses.UsersResponses;

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
    using System.Threading.Tasks;

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

        private readonly IMapper _mapper;
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
        /// <param name="mapper">The mapper.</param>
        public AccountsController(
            IControllerContext controllerContext,
            IRegistrationService registrationService,
            IAuthService authService,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IUserService userService,
            IEmailExtensionService emailService,
            IMapper mapper)
            : base(controllerContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService;
            _emailExtensionService = emailService;
            _mapper = mapper;
            _registrationService = registrationService;
            _authService = authService;
            // _refreshTokenService = refreshTokenService;
        }

        /// <summary>
        /// Get user data by user id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>user json.</returns>
        [HttpGet(ApiRoutes.AccountsController.Initialize)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult Initialize([FromRoute] string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return Bad("Something Went Wrong");
            }

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

            return Ok(_mapper.Map<List<AccountResponse>>(users));
        }

        /// <summary>
        /// Sends the verification email asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
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
        public async Task<IActionResult> PostAsync([FromBody] LoginRequest credentials)
        {
            if (!ModelState.IsValid)
            {
                return Bad(ModelState);
            }

            var jwt = await _authService.GetJwtAsync(credentials.Email, credentials.Password);
            if (jwt == null)
            {
                return Bad("Invalid name or password");
            }

            var user = await _authService.GetByUserNameAsync(credentials.Email);
            if (user is null)
            {
                return Bad("");
            }

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
        public async Task<IActionResult> CreateAsync([FromBody] RegistrationRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Bad(ModelState);
            }

            model.UserName = model.Email;

            var userIdentity = _mapper.Map<ApplicationUser>(model);
            userIdentity.CreatedOn = DateTime.Now;
            userIdentity.SecurityStamp = Guid.NewGuid().ToString();
            var result = await _registrationService.RegisterAsync(userIdentity, model.Password);

            if (!result.Succeeded)
            {
                return Bad(result);
            }

            if (model.Roles == null)
            {
                model.Roles = new[] {"User"};
            }
            
            foreach(var role in model.Roles)
            {
                var ir = await _userManager.AddToRoleAsync(userIdentity, role);
                if (!ir.Succeeded)
                {
                    return BadRequest();
                }
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
        public async Task<IActionResult> UpdateAsync([FromBody] ChangePasswordRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Bad(ModelState);
            }

            var changePasswordResult = await _userService.ChangePasswordAsync(CurrentUser.UserName, model.OldPassword, model.NewPassword);
            if (changePasswordResult.Succeeded)
            {
                return NoContent();
            }

            ModelState.AddModelError(changePasswordResult.Errors);

            return Bad(ModelState);
        }
    }
}