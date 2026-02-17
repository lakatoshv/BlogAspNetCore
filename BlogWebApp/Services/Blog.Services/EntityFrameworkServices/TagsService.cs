// <copyright file="TagsService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.EntityServices.EntityFrameworkServices;

using System.Linq;
using System.Threading.Tasks;
using Contracts.V1.Responses.Chart;
using Core.Helpers;
using Data.Models;
using Data.Repository;
using Blog.Services.Core;
using Blog.Services.Core.Dtos;
using Blog.Services.Core.Dtos.Posts;
using GeneralService;
using Interfaces;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Tags service.
/// </summary>
/// <seealso cref="GeneralService{Tag}" />
/// <seealso cref="ITagsService" />
/// <remarks>
/// Initializes a new instance of the <see cref="TagsService"/> class.
/// </remarks>
/// <param name="repo">The repo.</param>
public class TagsService(IRepository<Tag> repo)
    : GeneralService<Tag>(repo), ITagsService
{
    /// <inheritdoc cref="ITagsService"/>
    public async Task<TagsViewDto> GetTagsAsync(SearchParametersDto searchParameters)
    {
        var tags = new TagsViewDto();
        var tagsList = await this.Repository.TableNoTracking.ToListAsync();

        if (!string.IsNullOrEmpty(searchParameters.Search))
        {
            tagsList = tagsList.Where(tag => tag.Title.ToLower().Contains(searchParameters.Search.ToLower())).ToList();
        }

        var tagsCount = tagsList.Count;

        if (searchParameters.SortParameters.CurrentPage != null && searchParameters.SortParameters.PageSize != null)
        {
            tagsList = tagsList.AsQueryable().OrderBy(searchParameters.SortParameters)
                .Skip((searchParameters.SortParameters.CurrentPage.Value - 1) *
                      searchParameters.SortParameters.PageSize.Value)
                .Take(searchParameters.SortParameters.PageSize.Value).ToList();
        }

        tags.Tags = tagsList.Select(x => new TagViewDto
        {
            Id = x.Id,
            Title = x.Title,
        }).ToList();

        if (searchParameters.SortParameters.CurrentPage != null && searchParameters.SortParameters.PageSize != null)
        {
            tags.PageInfo = new PageInfo
            {
                PageNumber = searchParameters.SortParameters.CurrentPage.Value,
                PageSize = searchParameters.SortParameters.PageSize.Value,
                TotalItems = tagsCount,
            };
        }

        return tags;
    }

    /// <inheritdoc cref="IPostsService"/>
    public async Task<ChartDataModel> GetTagsActivity()
        => new ()
        {
            Name = "Posts",
            Series = await Repository.TableNoTracking
                .GroupBy(x => x.Title)
                .Select(x => new ChartItem
                {
                    Name = x.Key,
                    Value = x.Count(),
                })
                .ToListAsync(),
        };
}