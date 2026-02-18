// <copyright file="IPostsDapperService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Blog.Contracts.V1.Responses.Chart;
using Blog.Data.Models;
using Blog.EntityServices.GeneralService;
using Blog.Services.Core.Dtos;
using Blog.Services.Core.Dtos.Posts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.EntityServices.DapperServices.Interfaces;

/// <summary>
/// Posts dapper service interfaces.
/// </summary>
public interface IPostsDapperService : IGeneralDapperService<Post>
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

    /// <summary>
    /// Asynchronous Get posts activity.
    /// </summary>
    /// <returns>Task.</returns>
    Task<ChartDataModel> GetPostsActivity();

    Task<byte[]> ExportPostsToExcel(PostsSearchParametersDto searchParameters);
}