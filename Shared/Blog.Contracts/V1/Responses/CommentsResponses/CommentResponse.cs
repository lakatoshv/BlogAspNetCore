namespace Blog.Contracts.V1.Responses.CommentsResponses
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Blog.Contracts.V1.Responses.UsersResponses;

    /// <summary>
    /// Comment response.
    /// </summary>
    public class CommentResponse
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets the post identifier.
        /// </summary>
        /// <value>
        /// The post identifier.
        /// </value>
        [Required]
        public int PostId { get; set; }

        /// <summary>
        /// Gets or sets the comment body.
        /// </summary>
        /// <value>
        /// The comment body.
        /// </value>
        [Required]
        public string CommentBody { get; set; }

        /// <summary>
        /// Gets or sets the created at.
        /// </summary>
        /// <value>
        /// The created at.
        /// </value>
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the likes.
        /// </summary>
        /// <value>
        /// The likes.
        /// </value>
        public int Likes { get; set; }

        /// <summary>
        /// Gets or sets the dislikes.
        /// </summary>
        /// <value>
        /// The dislikes.
        /// </value>
        public int Dislikes { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public virtual ApplicationUserResponse User { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }
    }
}