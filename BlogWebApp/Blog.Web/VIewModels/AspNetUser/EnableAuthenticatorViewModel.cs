namespace BLog.Web.ViewModels.AspNetUser;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

/// <summary>
/// Enable authenticator view model.
/// </summary>
public class EnableAuthenticatorViewModel
{
    /// <summary>
    /// Gets or sets code.
    /// </summary>
    [Required]
    [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Text)]
    [Display(Name = "Verification Code")]
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets sharedKey.
    /// </summary>
    [BindNever]
    public string SharedKey { get; set; }

    /// <summary>
    /// Gets or sets authenticationUri.
    /// </summary>
    [BindNever]
    public string AuthenticatorUri { get; set; }
}