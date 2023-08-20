namespace Blog.Contracts.V1.Requests.UsersRequests;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Interfaces;

/// <summary>
/// Registration request.
/// </summary>
public class RegistrationRequest : IRequest
{
    /// <summary>
    /// Gets or sets email.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets password.
    /// </summary>
    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets confirmPassword.
    /// </summary>
    [Required]
    public string ConfirmPassword { get; set; }

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
    /// Gets or sets userName.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets phoneNumber.
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets concurrencyStamp.
    /// </summary>
    public string ConcurrencyStamp { get; set; }

    /// <summary>
    /// Gets or sets roles.
    /// </summary>
    public IEnumerable<string> Roles { get; set; }
}