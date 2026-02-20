// <copyright file="ITagsDapperService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Blog.Contracts.V1.Responses.Chart;
using Blog.Data.Models;
using Blog.EntityServices.GeneralService;
using Blog.Services.Core.Dtos;
using Blog.Services.Core.Dtos.Posts;
using System.Threading.Tasks;

namespace Blog.EntityServices.DapperServices.Interfaces;

/// <summary>
/// Tags dapper service interface.
/// </summary>
/// <seealso cref="IGeneralDapperService{Tag}" />
public interface ITagsDapperService : IGeneralDapperService<Tag>
{
    /// <summary>
    /// Gets the tags asynchronous.
    /// </summary>
    /// <param name="searchParameters">The search parameters.</param>
    /// <returns>Task.</returns>
    Task<TagsViewDto> GetTagsAsync(SearchParametersDto searchParameters);

    /// <summary>
    /// Asynchronous Get tags activity.
    /// </summary>
    /// <returns>Task.</returns>
    Task<ChartDataModel> GetTagsActivity();
}