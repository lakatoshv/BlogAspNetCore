using System.ComponentModel.DataAnnotations;

namespace BLog.Web.ViewModels.AspNetUser
{
    /// <summary>
    /// Forgot password view model
    /// </summary>
    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// Gets or sets email.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
