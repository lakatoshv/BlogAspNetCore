// <copyright file="IProfileService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Interfaces
{
    using Data.Models;
    using GeneralService;

    /// <summary>
    /// Profile service interface.
    /// </summary>
    /// <seealso cref="Blog.Services.GeneralService.IGeneralService{Profile}" />
    public interface IProfileService : IGeneralService<Profile>
    {
    }
}