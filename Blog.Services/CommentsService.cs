// <copyright file="CommentsService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services
{
    using Data.Models;
    using Data.Repository;
    using GeneralService;
    using Interfaces;

    /// <summary>
    /// Comments service.
    /// </summary>
    /// <seealso cref="GeneralService{Comment}" />
    /// <seealso cref="ICommentsService" />
    public class CommentsService : GeneralService<Comment>, ICommentsService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsService"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        public CommentsService(
                IRepository<Comment> repo)
            : base(repo)
        {
        }
    }
}