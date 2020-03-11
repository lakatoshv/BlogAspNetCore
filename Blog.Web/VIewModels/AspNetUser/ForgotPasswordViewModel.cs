using System.ComponentModel.DataAnnotations;

namespace BLog.Web.ViewModels.AspNetUser
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
