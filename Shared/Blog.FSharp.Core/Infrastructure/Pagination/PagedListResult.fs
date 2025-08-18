namespace Blog.FSharp.Core.Infrastructure.Pagination

open System.Collections.Generic

/// <summary>
/// Paged list.
/// </summary>
/// <typeparam name="TEntity">TEntity.</typeparam>
type PagedListResult<'TEntity>() =
    /// <summary>
    /// Gets or sets a value indicating whether has next value.
    /// </summary>
    member val HasNext = false with get, set

    /// <summary>
    /// Gets or sets a value indicating whether has previous value.
    /// </summary>
    member val HasPrevious = false with get, set

    /// <summary>
    /// Gets or sets count.
    /// </summary>
    member val Count = 0 with get, set

    /// <summary>
    /// Gets or sets entities.
    /// </summary>
    member val Entities : IEnumerable<'TEntity> = Seq.empty<'TEntity> :> IEnumerable<'TEntity> with get, set

