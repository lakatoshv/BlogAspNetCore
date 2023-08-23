namespace Blog.Contracts.V1.Responses.HealthChecks;

/// <summary>
/// Health check.
/// </summary>
public class HealthCheck
{
    /// <summary>
    /// Gets or sets the status.
    /// </summary>
    /// <value>
    /// The status.
    /// </value>
    public string Status { get; set; }

    /// <summary>
    /// Gets or sets the component.
    /// </summary>
    /// <value>
    /// The component.
    /// </value>
    public string Component { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>
    /// The description.
    /// </value>
    public string Description { get; set; }
}