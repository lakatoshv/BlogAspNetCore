// <copyright file="IPostsTagsRelationsService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Blog.Data.Models;
    using Blog.Services.GeneralService;

    /// <summary>
    /// Posts tags relations service interface.
    /// </summary>
    /// <seealso cref="IGeneralService{Message}" />
    public interface IPostsTagsRelationsService : IGeneralService<PostsTagsRelations>
    {
        /// <summary>
        /// Adds the tags to post.
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        /// <param name="postsTagsRelations">The posts tags relations.</param>
        /// <param name="tags">The tags.</param>
        /// <returns>Task.</returns>
        Task AddTagsToPost(int postId, List<PostsTagsRelations> postsTagsRelations, IEnumerable<Tag> tags);
    }
}