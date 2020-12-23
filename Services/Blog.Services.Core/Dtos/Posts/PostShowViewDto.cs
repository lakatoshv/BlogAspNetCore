// <copyright file="PostShowViewDto.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace Blog.Services.Core.Dtos.Posts
{
    /// <summary>
    /// Post show view dto.
    /// </summary>
    public class PostShowViewDto
    {
        /// <summary>
        /// Gets or sets post.
        /// </summary>
        public PostViewDto Post { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        public CommentsViewDto Comments { get; set; }

        // public Comment Comment { get; set; }
        // public int CommentsCount { get; set; }
        // public Profile Profile { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public IList<TagViewDto> Tags { get; set; }
    }
}