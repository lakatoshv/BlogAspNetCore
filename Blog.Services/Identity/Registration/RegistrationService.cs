// <copyright file="RegistrationService.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Services.Identity.Registration
{
    using System;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Blog.Data.Models;
    using Blog.Services.EmailServices.Interfaces;
    using Blog.Services.Identity.User;
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Registration service.
    /// </summary>
    public class RegistrationService : IRegistrationService
    {
        /// <summary>
        /// User service.
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        /// Email extension service.
        /// </summary>
        private readonly IEmailExtensionService emailExtensionService;

        // TODO: Investigate if it is possible to abstracted out userrManager and Application context should to interfaces

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationService"/> class.
        /// </summary>
        /// <param name="userService">userService.</param>
        /// <param name="emailExtensionService">emailExtensionService.</param>
        public RegistrationService(IUserService userService, IEmailExtensionService emailExtensionService)
        {
            this.userService = userService;
            this.emailExtensionService = emailExtensionService;
        }

        /// <inheritdoc/>
        public IdentityResult Register(ApplicationUser user, string password)
        {
            // TODO: Either implement or remove
            throw new NotImplementedException();
        }

        // TODO: Get rid of IdentityResult, introduce own IServiceResult

        /// <inheritdoc/>
        public async Task<IdentityResult> RegisterAsync(ApplicationUser user, string password)
        {
            user.UserName = Regex.Replace(user.UserName, @"\s+", " ").Trim();

            var result = await this.userService.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var token = await this.userService.GetEmailVerificationTokenAsync(user.UserName);

                // await _emailExtensionService.SendVerificationEmailAsync(user.UserName, token);
            }

            return result;
        }
    }
}
