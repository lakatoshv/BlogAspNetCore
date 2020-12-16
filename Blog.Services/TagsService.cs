// <copyright file="TagsService.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Services
{
    using Data.Models;
    using Data.Repository;
    using GeneralService;
    using Interfaces;

    /// <summary>
    /// Tags service.
    /// </summary>
    /// <seealso cref="GeneralService{Tag}" />
    /// <seealso cref="ITagsService" />
    public class TagsService : GeneralService<Tag>, ITagsService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagsService"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        public TagsService(
            IRepository<Tag> repo)
            : base(repo)
        {
        }
    }
}