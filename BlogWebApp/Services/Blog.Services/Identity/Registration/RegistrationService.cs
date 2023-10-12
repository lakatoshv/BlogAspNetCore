// <copyright file="RegistrationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.EntityServices.Identity.Registration;

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Data.Models;
using User;
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

    // private readonly IEmailExtensionService _emailExtensionService;

    // TODO: Investigate if it is possible to abstracted out userManager and Application context should to interfaces

    /// <summary>
    /// Initializes a new instance of the <see cref="RegistrationService"/> class.
    /// </summary>
    /// <param name="userService">userService.</param>
    public RegistrationService(IUserService userService)
    {
        this.userService = userService;
    }

    /// <inheritdoc cref="IRegistrationService"/>
    public IdentityResult Register(ApplicationUser user, string password)
    {
        // TODO: Either implement or remove
        throw new NotImplementedException();
    }

    // TODO: Get rid of IdentityResult, introduce own IServiceResult

    /// <inheritdoc cref="IRegistrationService"/>
    public async Task<IdentityResult> RegisterAsync(ApplicationUser user, string password)
    {
        user.UserName = Regex.Replace(user.UserName, @"\s+", " ").Trim();

        var result = await this.userService.CreateAsync(user, password);

        if (result.Succeeded)
        {
            // var token =
            await this.userService.GetEmailVerificationTokenAsync(user.UserName);

            // await _emailExtensionService.SendVerificationEmailAsync(user.UserName, token);
        }

        return result;
    }
}