// <copyright file="ITagsService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Interfaces
{
    using System.Threading.Tasks;
    using Blog.Data.Models;
    using Blog.Services.Core.Dtos;
    using Blog.Services.Core.Dtos.Posts;
    using Blog.Services.GeneralService;

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