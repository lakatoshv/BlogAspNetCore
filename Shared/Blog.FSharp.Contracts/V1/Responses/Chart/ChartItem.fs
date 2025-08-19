namespace Blog.FSharp.Contracts.V1.Responses.Chart

/// <summary>
/// Chart item.
/// </summary>
type ChartItem() =
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    member val Name: string = "" with get, set

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    member val Value: int = 0 with get, set

