namespace Blog.FSharp.Core.Infrastructure

open System.Linq
open Blog.FSharp.Core.Enums
open Blog.FSharp.Core.Infrastructure.Pagination.Interfaces

/// <summary>
/// Field sort order.
/// </summary>
/// <typeparam name="T">Type.</typeparam>
type FieldSortOrder<'T when 'T : not struct>(?name: string, ?direction: OrderType) =

    let mutable name = defaultArg name null
    let mutable direction = defaultArg direction OrderType.Ascending

    /// <summary>
    /// Gets or sets name.
    /// </summary>
    member this.Name
        with get() = name
        and set(value) = name <- value

    /// <summary>
    /// Gets or sets direction.
    /// </summary>
    member this.Direction
        with get() = direction
        and set(value) = direction <- value

    /// <inheritdoc cref="ISortCriteria{T}" />
    interface ISortCriteria<'T> with
        member this.Direction
            with get() = direction
            and set(value) = direction <- value

        member this.ApplyOrdering(qry: IQueryable<'T>, useThenBy: bool) =
            let descending = (direction = OrderType.Descending)
            if useThenBy then
                QueryableExtensions.ThenBy qry name descending
            else
                QueryableExtensions.OrderBy qry name descending