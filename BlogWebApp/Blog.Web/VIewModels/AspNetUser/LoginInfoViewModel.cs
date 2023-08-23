namespace BLog.Web.ViewModels.AspNetUser;

/// <summary>
/// Login info view model.
/// </summary>
public class LoginInfoViewModel
{
    /// <summary>
    /// Gets or sets jwt.
    /// </summary>
    public string Jwt { get; set; }

    /// <summary>
    /// Gets or sets profileId.
    /// </summary>
    public int ProfileId { get; set; }
}