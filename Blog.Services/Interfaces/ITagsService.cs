// <copyright file="ITagsService.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

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
    }
}