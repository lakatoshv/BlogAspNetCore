namespace Blog.FSharp.Contracts.V1.Responses

open System

/// <summary>
/// Page info response.
/// </summary>
type PageInfoResponse() =
    /// <summary>
    /// Gets or sets current page number.
    /// </summary>
    member val PageNumber: int = 0 with get, set

    /// <summary>
    /// Gets or sets page size.
    /// </summary>
    member val PageSize: int = 0 with get, set

    /// <summary>
    /// Gets or sets total items count.
    /// </summary>
    member val TotalItems: int = 0 with get, set

    /// <summary>
    /// Gets total pages count.
    /// </summary>
    member this.TotalPages
        with get() = int (Math.Ceiling(decimal this.TotalItems / decimal this.PageSize))