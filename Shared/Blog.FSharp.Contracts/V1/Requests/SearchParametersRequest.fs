namespace Blog.FSharp.Contracts.V1.Requests

open Blog.FSharp.Contracts.V1.Requests.Interfaces

/// <summary>
/// Search parameters request.
/// </summary>
type SearchParametersRequest() =
    interface IRequest

    /// <summary>
    /// Gets or sets search.
    /// </summary>
    member val Search : string = null with get, set

    /// <summary>
    /// Gets or sets the sort parameters.
    /// </summary>
    /// <value>
    /// The sort parameters.
    /// </value>
    member val SortParameters : SortParametersRequest|null = null with get, set