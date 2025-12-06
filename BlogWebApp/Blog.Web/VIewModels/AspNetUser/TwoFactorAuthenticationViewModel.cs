namespace BLog.Web.ViewModels.AspNetUser;

/// <summary>
/// Two-factor authentication view model
/// </summary>
public class TwoFactorAuthenticationViewModel
{
    /// <summary>
    /// Gets or sets hasAuthenticator.
    /// </summary>
    public bool HasAuthenticator { get; set; }

    /// <summary>
    /// Gets or sets recoveryCodesLeft.
    /// </summary>
    public int RecoveryCodesLeft { get; set; }

    /// <summary>
    /// Gets or sets is2FaEnabled.
    /// </summary>
    public bool Is2FaEnabled { get; set; }
}