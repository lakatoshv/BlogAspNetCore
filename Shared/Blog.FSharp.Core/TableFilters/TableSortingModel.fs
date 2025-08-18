namespace Blog.FSharp.Core.TableFilters

/// <summary>
/// Represents a sorting instruction for a table.
/// </summary>
[<CLIMutable>]
type TableSortingModel = {
    /// <summary>
    /// Index of the column to sort by (zero-based).
    /// </summary>
    Column : int

    /// <summary>
    /// Sorting direction ("asc" or "desc").
    /// </summary>
    Dir : string
}