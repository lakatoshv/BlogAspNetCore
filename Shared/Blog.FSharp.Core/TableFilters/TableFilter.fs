namespace Blog.FSharp.Core.TableFilters

open System
open System.Collections.Generic
open System.Linq
open Blog.FSharp.Core.Enums

/// <summary>
/// Table filter.
/// </summary>
type TableFilter() =
    /// <summary>
    /// Gets or sets start.
    /// </summary>
    member val Start : int = 0 with get, set

    /// <summary>
    /// Gets or sets length.
    /// </summary>
    member val Length : int = 0 with get, set

    /// <summary>
    /// Gets or sets search.
    /// </summary>
    member val Search : TableSearchModel|null = null with get, set

    /// <summary>
    /// Gets or sets order.
    /// </summary>
    member val Order : IEnumerable<TableSortingModel> = Seq.empty with get, set

    /// <summary>
    /// Gets or sets columns.
    /// </summary>
    member val Columns : List<TableColumn> = new List<TableColumn>() with get, set

    /// <summary>
    /// Gets page count.
    /// </summary>
    member this.PageCount
        with get() = (this.Start / this.Length) + 1

    /// <summary>
    /// Gets page size.
    /// </summary>
    member this.PageSize
        with get() = this.Length

    /// <summary>
    /// Gets order type.
    /// </summary>
    member this.OrderType
        with get() =
            match Seq.tryHead this.Order with
            | Some first when first.Dir = "asc" ->
                OrderType.Ascending
            | _ ->
                OrderType.Descending

        /// <summary>
        /// Gets column name.
        /// </summary>
        member this.ColumnName
            with get() =
                match Seq.tryHead this.Order with
                | Some first when first.Column >= 0 && first.Column < this.Columns.Count ->
                    this.Columns.[first.Column].Data
                | _ ->
                    String.Empty

