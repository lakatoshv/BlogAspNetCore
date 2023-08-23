namespace BLog.Web.ViewModels.Manage;

using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

/// <summary>
/// Index view model.
/// </summary>
public class IndexViewModel
{
    /// <summary>
    /// Gets or sets hasPassword.
    /// </summary>
    public bool HasPassword { get; set; }

    /// <summary>
    /// Gets or sets logins.
    /// </summary>
    public IList<UserLoginInfo> Logins { get; set; }

    /// <summary>
    /// Gets or sets pPhoneNumber.
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets tTwoFactor.
    /// </summary>
    public bool TwoFactor { get; set; }

    /// <summary>
    /// Gets or sets browserRemembered.
    /// </summary>
    public bool BrowserRemembered { get; set; }
}