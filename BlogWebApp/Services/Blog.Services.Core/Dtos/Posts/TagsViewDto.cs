// <copyright file="TagsViewDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Dtos.Posts
{
    using System.Collections.Generic;
    using Blog.Core.Helpers;

    /// <summary>
    /// Tags view dto.
    /// </summary>
    public class TagsViewDto
    {
        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public IList<TagViewDto> Tags { get; set; }

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