// <copyright file="IProfileService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Interfaces
{
    using System.Threading.Tasks;
    using Blog.Data.Models;
    using Blog.Services.Core.Dtos.User;
    using Blog.Services.GeneralService;

    /// <summary>
    /// Profile service interface.
    /// </summary>
    /// <seealso cref="IGeneralService{Profile}" />
    public interface IProfileService : IGeneralService<Profile>
    {
        /// <summary>
        /// Gets the profile.
        /// </summary>
        /// <param name="profileId">The profile identifier.</param>
        /// <returns>Task.</returns>
        Task<ApplicationUserDto> GetProfile(int profileId);
    }
}