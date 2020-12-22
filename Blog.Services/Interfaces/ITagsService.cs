// <copyright file="ITagsService.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Blog.Services.Core.Dtos;
using Blog.Services.Core.Dtos.Posts;

namespace Blog.Services.Interfaces
{
    using Data.Models;
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
    }
}