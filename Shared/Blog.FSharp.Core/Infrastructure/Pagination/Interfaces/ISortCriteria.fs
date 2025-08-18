namespace Blog.FSharp.Core.Infrastructure.Pagination.Interfaces

open System.Linq
open Blog.FSharp.Core.Enums

/// <summary>
/// Sort items by criteria interface.
/// </summary>
/// <typeparam name="T">Type.</typeparam>
type ISortCriteria<'T> =
    /// <summary>
    /// Gets or sets direction.
    /// </summary>
    abstract member Direction : OrderType with get, set

    /// <summary>
    /// Apply ordering.
    /// </summary>
    /// <param name="query">query.</param>
    /// <param name="useThenBy">useThenBy.</param>
    /// <returns>IOrderedQueryable.</returns>
    abstract member ApplyOrdering : query: IQueryable<'T> * useThenBy: bool -> IOrderedQueryable<'T>