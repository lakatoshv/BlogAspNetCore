namespace BLog.Web.ViewModels.AspNetUser;

using System.ComponentModel.DataAnnotations;

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