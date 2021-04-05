namespace Blog.Contracts.V1.Requests.UsersRequests
{
    using System.ComponentModel.DataAnnotations;
    using Blog.Contracts.V1.Requests.Interfaces;

    /// <summary>
    /// Login request.
    /// </summary>
    public class LoginRequest : IRequest
    {
        /// <summary>
        /// Gets or sets email.
        /// </summary>
        [Required]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        [Required]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Password { get; set; }
    }
}