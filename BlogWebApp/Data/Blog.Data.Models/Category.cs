// <copyright file="Category.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Data.Models
{
    using Core;

    /// <summary>
    /// Category entity.
    /// </summary>
    /// <seealso cref="Entity" />
    public class Category : Entity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the posts.
        /// </summary>
        /// <value>
        /// The posts.
        /// </value>
        public virtual ICollection<Post> Posts { get; set; }
    }
}