﻿// <copyright file="ICommentsService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Interfaces;

using System.Threading.Tasks;
using Contracts.V1.Responses.Chart;
using Data.Models;
using Core.Dtos;
using Core.Dtos.Posts;
using GeneralService;

/// <summary>
/// Comments service interface.
/// </summary>
/// <seealso cref="IGeneralService{Comment}" />
public interface ICommentsService : IGeneralService<Comment>
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
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<Comment> GetCommentAsync(int id);

    /// <summary>
    /// Gets the paged comments.
    /// </summary>
    /// <param name="sortParameters">The sort parameters.</param>
    /// <returns></returns>
    Task<CommentsViewDto> GetPagedComments(SortParametersDto sortParameters);

    /// <summary>
    /// Asynchronous Get comments activity.
    /// </summary>
    /// <returns>Task.</returns>
    Task<ChartDataModel> GetCommentsActivity();
}