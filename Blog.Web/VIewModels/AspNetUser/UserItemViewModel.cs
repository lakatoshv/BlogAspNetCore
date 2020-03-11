using Blog.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Blog.Web.ViewModels.AspNetUser
{
    public class UserItemViewModel
    {
        public UserItemViewModel()
        {

        }

        public UserItemViewModel(ApplicationUser user, int currentWorkspaceId)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            UserName = user.UserName;
        }

        public string Id { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 2)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 2)]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
    }
}