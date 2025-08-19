namespace Blog.FSharp.Contracts.V1.Responses.Chart.HealthChecks

/// <summary>
/// Health check.
/// </summary>
type HealthCheck() =
    /// <summary>
    /// Gets or sets the status.
    /// </summary>
    member val Status: string = "" with get, set

    /// <summary>
    /// Gets or sets the component.
    /// </summary>
    member val Component: string = "" with get, set

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    member val Description: string = "" with get, set

