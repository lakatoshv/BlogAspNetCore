// <copyright file="PostViewDto.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Dtos.Posts
{
    using System;
    using System.Collections.Generic;
    using User;
    using Data.Models;

    /// <summary>
    /// Post view dto.
    /// </summary>
    public class PostViewDto
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets seen.
        /// </summary>
        public int Seen { get; set; }

        /// <summary>
        /// Gets or sets likes.
        /// </summary>
        public int Likes { get; set; }

        /// <summary>
        /// Gets or sets dislikes.
        /// </summary>
        public int Dislikes { get; set; }

        /// <summary>
        /// Gets or sets image url.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets author id.
        /// </summary>
        public string AuthorId { get; set; }

        /// <summary>
        /// Gets or sets application user.
        /// </summary>
        public virtual ApplicationUserDto Author { get; set; }

        /// <summary>
        /// Gets or sets created at.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the comments count.
        /// </summary>
        /// <value>
        /// The comments count.
        /// </value>
        public int CommentsCount { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        public IList<Comment> Comments { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public IList<TagViewDto> Tags { get; set; }
    }
}
