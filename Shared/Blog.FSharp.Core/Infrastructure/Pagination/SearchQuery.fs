namespace Blog.FSharp.Core.Infrastructure.Pagination

open System
open System.Collections.Generic
open System.Linq.Expressions
open Blog.FSharp.Core.Infrastructure.Pagination.Interfaces

/// <summary>
/// Search query.
/// </summary>
/// <typeparam name="TEntity">TEntity.</typeparam>
type SearchQuery<'TEntity>() =

    let filters = List<Expression<Func<'TEntity, bool>>>()
    let sortCriterias = List<ISortCriteria<'TEntity>>()

    /// <summary>
    /// Gets or sets sort criterias.
    /// </summary>
    member val SortCriterias : List<ISortCriteria<'TEntity>> = sortCriterias with get, set

    /// <summary>
    /// Gets or sets include properties.
    /// </summary>
    member val IncludeProperties : string = null with get, set

    /// <summary>
    /// Gets or sets skip.
    /// </summary>
    member val Skip = 0 with get, set

    /// <summary>
    /// Gets or sets take.
    /// </summary>
    member val Take = 0 with get, set

    /// <summary>
    /// Gets or sets filters.
    /// </summary>
    member val Filters : List<Expression<Func<'TEntity, bool>>> = filters with get, set

    /// <summary>
    /// Add filter.
    /// </summary>
    member this.AddFilter(filter: Expression<Func<'TEntity, bool>>) =
        this.Filters.Add(filter)

    /// <summary>
    /// Add sort criteria params.
    /// </summary>
    member this.AddSortCriteria(sortCriteria: ISortCriteria<'TEntity>) =
        this.SortCriterias.Add(sortCriteria)