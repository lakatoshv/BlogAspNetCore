using System.ComponentModel.DataAnnotations;

namespace BLog.ViewModels.AspNetUser
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Email { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Password { get; set; }
    }
}
