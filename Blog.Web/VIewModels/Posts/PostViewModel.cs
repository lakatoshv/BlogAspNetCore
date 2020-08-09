namespace Blog.Web.VIewModels.Posts
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Data.Models;

    /// <summary>
    /// Post view model
    /// </summary>
    public class PostViewModel
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets title.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets content.
        /// </summary>
        [Required]
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
        public virtual ApplicationUser Author { get; set; }

        /// <summary>
        /// Gets or sets created at.
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        public string[] Tags { get; set; }

        // public int CommentsCount { get; set; }
        // public IList<Comment> Comments { get; set; }

        /// <summary>
        /// Update method.
        /// </summary>
        public Post Update(Post model)
        {
            model.Title = Title;
            model.Description = Description;
            model.Content = Content;
            model.ImageUrl = ImageUrl;
            // model.Tags = Tags;
            model.AuthorId = AuthorId;
            return model;
        }
    }
}
