// <copyright file="ICommentsDapperService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Blog.Contracts.V1.Responses.Chart;
using Blog.EntityServices.GeneralService;
using Blog.Services.Core.Dtos;
using Blog.Services.Core.Dtos.Posts;
using System.Threading.Tasks;
using Blog.Data.Models;

namespace Blog.EntityServices.DapperServices.Interfaces;

/// <summary>
/// Comments service interface.
/// </summary>
/// <seealso cref="IGeneralDapperService{T}" />
public interface ICommentsDapperService : IGeneralDapperService<Comment>
{
    /// <summary>
    /// Gets the paged comments by post identifier.
    /// </summary>
    /// <param name="postId">The post identifier.</param>
    /// <param name="sortParameters">The sort parameters.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<CommentsViewDto> GetPagedCommentsByPostId(int postId, SortParametersDto sortParameters);

    /// <summary>
    /// Gets the comment asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>A <see cref="Task"/>The representing the asynchronous operation.</returns>
    Task<Comment> GetCommentAsync(int id);

    /// <summary>
    /// Gets the paged comments.
    /// </summary>
    /// <param name="sortParameters">The sort parameters.</param>
    /// <returns>A <see cref="Task"/>The representing the asynchronous operation.</returns>
    Task<CommentsViewDto> GetPagedComments(SortParametersDto sortParameters);

    /// <summary>
    /// Asynchronous Get comments activity.
    /// </summary>
    /// <returns>Task.</returns>
    Task<ChartDataModel> GetCommentsActivity();
}