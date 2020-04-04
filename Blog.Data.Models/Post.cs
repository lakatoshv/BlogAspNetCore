﻿namespace Blog.Data.Models
{
    using System;
    using Core;

    /// <summary>
    /// Post model.
    /// </summary>
    public class Post : Entity
    {
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
        /// Gets or sets tags.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Gets or sets author id.
        /// </summary>
        public string AuthorId { get; set; }

        /// <summary>
        /// Gets or sets application user.
        /// </summary>
        public virtual ApplicationUser Author { get; set; }

        /// <summary>
        /// Gets or sets created at.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}