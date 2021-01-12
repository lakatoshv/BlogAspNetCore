using System.ComponentModel.DataAnnotations;

namespace BLog.Web.ViewModels.AspNetUser
{
    /// <summary>
    /// Reset password view model.
    /// </summary>
    public class ResetPasswordViewModel
    {
        /// <summary>
        /// Gets or sets userName.
        /// </summary>
        [Required]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets token.
        /// </summary>
        [Required]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets newPassword.
        /// </summary>
        [Required]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets confirmPassword.
        /// </summary>
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
