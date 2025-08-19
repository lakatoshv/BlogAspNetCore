namespace Blog.FSharp.Contracts.V1.Responses.Chart

open System.Collections.Generic

/// <summary>
/// Chart data model.
/// </summary>
type ChartDataModel() =
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    member val Name: string = "" with get, set

    /// <summary>
    /// Gets or sets the series.
/// </summary>
    member val Series: List<ChartItem> = List<ChartItem>() with get, set

