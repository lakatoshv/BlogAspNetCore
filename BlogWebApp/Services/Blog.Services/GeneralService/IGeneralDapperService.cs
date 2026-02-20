// <copyright file="IGeneralDapperService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Blog.Core;
using Blog.Core.Infrastructure.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.EntityServices.GeneralService;

/// <summary>
/// General dappers service interface.
/// </summary>
/// <typeparam name="T">Type.</typeparam>
public interface IGeneralDapperService<T>
    where T : IEntity
{
    /// <summary>
    /// Async find item by id.
    /// </summary>
    /// <param name="id">id.</param>
    /// <returns>Task.</returns>
    Task<T?> FindAsync(object id);

    /// <summary>
    /// Inserts the asynchronous.
    /// </summary>
    /// <param name="entity">The enumerable.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task InsertAsync(T entity);

    /// <summary>
    /// Inserts the asynchronous.
    /// </summary>
    /// <param name="entities">The enumerable.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task InsertAsync(IEnumerable<T> entities);

    /// <summary>
    /// Update item.
    /// </summary>
    /// <param name="entity">entity.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Delete item.
    /// </summary>
    /// <param name="id">entity.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteAsync(object id);

    /// <summary>
    /// Gets all asynchronous.
    /// </summary>
    /// <returns>Task.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Asynchronous check on any by specification.
    /// </summary>
    /// <param name="whereClause">The where clause.</param>
    /// <param name="param">The param.</param>
    /// <returns>Task.</returns>
    Task<bool> AnyAsync(string whereClause, object? param = null);

    /// <summary>
    /// Async search.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <returns>Task.</returns>
    Task<PagedListResult<T>> SearchAsync(DapperSearchQuery query);
}