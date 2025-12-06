// <copyright file="IRegistrationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.EntityServices.Identity.Registration;

using Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

/// <summary>
/// Registration service interface.
/// </summary>
public interface IRegistrationService
{
    // TODO: Refactor, no need to return Identity Result. Just a bool (.IsSuccess) should be sufficient.
    // This will ubind us from Microsoft.AspNetCore.Identity

    /// <summary>
    /// Register user.
    /// </summary>
    /// <param name="user">user.</param>
    /// <param name="password">password.</param>
    /// <returns>IdentityResult.</returns>
    IdentityResult Register(ApplicationUser user, string password);

    /// <summary>
    /// Async register user.
    /// </summary>
    /// <param name="user">user.</param>
    /// <param name="password">password.</param>
    /// <returns>Task.</returns>
    Task<IdentityResult> RegisterAsync(ApplicationUser user, string password);
}