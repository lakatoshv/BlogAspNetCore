namespace BLog.Web.ViewModels.Manage;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Change password view model.
/// </summary>
public class ChangePasswordViewModel
{
    /// <summary>
    /// Gets or sets oldPassword.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Current password")]
    public string OldPassword { get; set; }

    /// <summary>
    /// Gets or sets newPassword.
    /// </summary>
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "New password")]
    public string NewPassword { get; set; }
}