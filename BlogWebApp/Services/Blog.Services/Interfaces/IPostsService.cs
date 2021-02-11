// <copyright file="IPostsService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Blog.Data.Models;
    using Blog.Services.Core.Dtos;
    using Blog.Services.Core.Dtos.Posts;
    using Blog.Services.GeneralService;

    /// <summary>
    /// Posts service interfaces
    /// </summary>
    public interface IPostsService : IGeneralService<Post>
    {
        /// <summary>
        /// Async get posts async.
        /// </summary>
        /// <param name="searchParameters">searchParameters.</param>
        /// <returns>Task.</returns>
        Task<PostsViewDto> GetPostsAsync(PostsSearchParametersDto searchParameters);

        /// <summary>
        /// Async get post.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>Task.</returns>
        Task<Post> GetPostAsync(int id);

        /// <summary>
        /// Async get post with comment.
        /// </summary>
        /// <param name="postId">postId.</param>
        /// <param name="sortParameters">sortParameters.</param>
        /// <returns>Task.</returns>
        Task<PostShowViewDto> GetPost(int postId, SortParametersDto sortParameters);

        /// <summary>
        /// Async get user posts.
        /// </summary>
        /// <param name="userId">userId.</param>
        /// <param name="searchParameters">searchParameters.</param>
        /// <returns>Task.</returns>
        Task<PostsViewDto> GetUserPostsAsync(string userId, PostsSearchParametersDto searchParameters);

        /// <summary>
        /// Inserts the asynchronous.
        /// </summary>
        /// <param name="post">The post.</param>
        /// <param name="tags">The tags.</param>
        /// <returns>Task.</returns>
        Task InsertAsync(Post post, IEnumerable<Tag> tags);
    }
}