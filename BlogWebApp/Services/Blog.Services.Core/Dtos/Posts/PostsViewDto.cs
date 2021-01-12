// <copyright file="PostsViewDto.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Dtos.Posts
{
    using System.Collections.Generic;
    using Blog.Core.Helpers;

    /// <summary>
    /// Posts view dto.
    /// </summary>
    public class PostsViewDto
    {
        /// <summary>
        /// Gets or sets posts.
        /// </summary>
        public IList<PostViewDto> Posts { get; set; }

        /// <summary>
        /// Gets or sets display type.
        /// </summary>
        public string DisplayType { get; set; }

        /// <summary>
        /// Gets or sets page info.
        /// </summary>
        public PageInfo PageInfo { get; set; }
    }
}