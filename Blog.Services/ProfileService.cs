// <copyright file="ProfileService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services
{
    using Data.Models;
    using Data.Repository;
    using GeneralService;
    using Interfaces;

    /// <summary>
    /// Profile service.
    /// </summary>
    /// <seealso cref="GeneralService{Profile}" />
    /// <seealso cref="IProfileService" />
    public class ProfileService : GeneralService<Profile>, IProfileService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileService"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        public ProfileService(
            IRepository<Profile> repo)
            : base(repo)
        {
        }

    }
}