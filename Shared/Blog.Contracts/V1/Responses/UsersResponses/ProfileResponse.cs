namespace Blog.Contracts.V1.Responses.UsersResponses;

/// <summary>
/// Profile response.
/// </summary>
public class ProfileResponse
{
    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    /// <value>
    /// The user identifier.
    /// </value>
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the user.
    /// </summary>
    /// <value>
    /// The user.
    /// </value>
    public virtual ApplicationUserResponse User { get; set; }

    /// <summary>
    /// Gets or sets the About.
    /// </summary>
    /// <value>
    /// The About.
    /// </value>
    public string About { get; set; }

    /// <summary>
    /// Gets or sets the profile img.
    /// </summary>
    /// <value>
    /// The profile img.
    /// </value>
    public string ProfileImg { get; set; }
}