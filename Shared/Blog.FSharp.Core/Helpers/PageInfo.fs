namespace Blog.FSharp.Core.Helpers

open System

/// <summary>
/// Page information.
/// </summary>
type PageInfo() =
    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    member val PageNumber: int = 0 with get, set

    /// <summary>
    /// Gets or sets the page size.
    /// </summary>
    member val PageSize: int = 0 with get, set

    /// <summary>
    /// Gets or sets the total number of items.
    /// </summary>
    member val TotalItems: int = 0 with get, set

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    member this.TotalPages =
        if this.PageSize = 0 then 0
        else int (Math.Ceiling(decimal this.TotalItems / decimal this.PageSize))

