// <copyright file="IProfileDapperService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Blog.Data.Models;
using Blog.EntityServices.GeneralService;
using Blog.Services.Core.Dtos.User;
using System.Threading.Tasks;

namespace Blog.EntityServices.DapperServices.Interfaces;

/// <summary>
/// Profile dapper service interface.
/// </summary>
/// <seealso cref="IGeneralDapperService{Profile}" />
public interface IProfileDapperService : IGeneralDapperService<Profile>
{
    /// <summary>
    /// Gets the profile.
    /// </summary>
    /// <param name="profileId">The profile identifier.</param>
    /// <returns>Task.</returns>
    Task<ApplicationUserDto> GetProfile(int profileId);
}