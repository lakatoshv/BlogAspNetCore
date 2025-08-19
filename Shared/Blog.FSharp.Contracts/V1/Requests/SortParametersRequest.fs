namespace Blog.FSharp.Contracts.V1.Requests

open System
open Blog.FSharp.Contracts.V1.Requests.Interfaces

/// <summary>
/// Sort Parameters request.
/// </summary>
type SortParametersRequest() =
    interface IRequest

    /// <summary>
    /// Gets or sets orderBy.
    /// </summary>
    member val OrderBy : string = null with get, set

    /// <summary>
    /// Gets or sets sortBy.
    /// </summary>
    member val SortBy : string = null with get, set

    /// <summary>
    /// Gets or sets currentPage.
    /// </summary>
    member val CurrentPage : Nullable<int> = Nullable() with get, set

    /// <summary>
    /// Gets or sets pageSize.
    /// </summary>
    member val PageSize : Nullable<int> = Nullable() with get, set

    /// <summary>
    /// Gets or sets displayType.
    /// </summary>
    member val DisplayType : string = null with get, set

