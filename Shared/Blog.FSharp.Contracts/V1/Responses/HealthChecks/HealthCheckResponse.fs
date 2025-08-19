namespace Blog.FSharp.Contracts.V1.Responses.Chart.HealthChecks

open System
open System.Collections.Generic

/// <summary>
/// Health check response.
/// </summary>
type HealthCheckResponse() =
    /// <summary>
    /// Gets or sets the status.
    /// </summary>
    member val Status: string = "" with get, set

    /// <summary>
    /// Gets or sets the checks.
    /// </summary>
    member val Checks: IEnumerable<HealthCheck> = Seq.empty with get, set

    /// <summary>
    /// Gets or sets the duration.
    /// </summary>
    member val Duration: TimeSpan = TimeSpan.Zero with get, set

