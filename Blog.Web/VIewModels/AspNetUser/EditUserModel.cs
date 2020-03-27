using System.ComponentModel.DataAnnotations;
using Blog.Data.Models;

namespace BLog.Web.ViewModels.AspNetUser
{
    /// <summary>
    /// Edit user view model.
    /// </summary>
    public class EditUserModel
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets email.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        // public string Password { get; set; }
        // public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets firstName.
        /// </summary>
        [Required]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets lastName.
        /// </summary>
        [Required]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets phoneNumber.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Update user.
        /// </summary>
        /// <param name="user">user.</param>
        /// <returns>ApplicationUser.</returns>
        public ApplicationUser Update(ApplicationUser user)
        {
            user.Email = Email;
            user.UserName = FirstName + " " + LastName;
            user.PhoneNumber = PhoneNumber;
            return user;
        }
    }
}
   
