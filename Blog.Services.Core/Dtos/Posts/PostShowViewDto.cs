// <copyright file="PostShowViewDto.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Dtos.Posts
{
    using Data.Models;

    /// <summary>
    /// Post show view dto.
    /// </summary>
    public class PostShowViewDto
    {
        /// <summary>
        /// Gets or sets post.
        /// </summary>
        public Post Post { get; set; }

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
    }
}