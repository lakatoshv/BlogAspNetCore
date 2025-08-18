namespace Blog.FSharp.Core.TableFilters

/// <summary>
/// Table column.
/// </summary>
type TableColumn() =
    /// <summary>
    /// Gets or sets data.
    /// </summary>
    member val Data : string = null with get, set