using System.ComponentModel.DataAnnotations;
using Blog.Data.Models;

namespace BLog.Web.ViewModels.AspNetUser
{
    public class EditUserModel
    {
        public string Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        //public string Password { get; set; }
        //public string ConfirmPassword { get; set; }
        
        [Required]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
     
        public ApplicationUser Update(ApplicationUser user)
        {
            user.Email = Email;
            user.UserName = FirstName + " " + LastName;
            user.PhoneNumber = PhoneNumber;
            return user;
        }
    }
}
   
