// <copyright file="ITagsService.cs" company="PlaceholderCompany">
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
/// Tags service interface.
/// </summary>
/// <seealso cref="IGeneralService{Profile}" />
public interface ITagsService : IGeneralService<Tag>
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