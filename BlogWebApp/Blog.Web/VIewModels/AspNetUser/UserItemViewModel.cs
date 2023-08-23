namespace Blog.Web.ViewModels.AspNetUser;

using System.ComponentModel.DataAnnotations;
using Data.Models;

/// <summary>
/// User item view model.
/// </summary>
public class UserItemViewModel
{
    /// <summary>
    /// Initializes static members of the <see cref="UserItemViewModel"/> class.
    /// </summary>
    public UserItemViewModel()
    {
    }

    /// <summary>
    /// Initializes static members of the <see cref="UserItemViewModel"/> class.
    /// </summary>
    /// <param name="user">user.</param>
    public UserItemViewModel(ApplicationUser user)
    {
        Id = user.Id;
        UserName = user.UserName;
        Email = user.Email;
        PhoneNumber = user.PhoneNumber;
        UserName = user.UserName;
    }

    /// <summary>
    /// Gets or sets id.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets userName.
    /// </summary>
    [Required]
    [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
        MinimumLength = 2)]
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets email.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets firstName.
    /// </summary>
    [Required]
    [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
        MinimumLength = 2)]
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets lastName.
    /// </summary>
    [Required]
    [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
        MinimumLength = 2)]
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets phoneNumber.
    /// </summary>
    public string PhoneNumber { get; set; }
}