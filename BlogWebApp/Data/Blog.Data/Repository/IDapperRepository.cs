// <copyright file="IDapperRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Core;
using Blog.Core.Infrastructure.Pagination;

namespace Blog.Data.Repository;

/// <summary>
/// Dapper repository interface.
/// </summary>
/// <typeparam name="T">Type.</typeparam>
public interface IDapperRepository<T>
    where T : IEntity
{
    /// <summary>
    /// Gets all asynchronous.
    /// </summary>
    /// <returns>Type.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Async get item by id async.
    /// </summary>
    /// <param name="id">id.</param>
    /// <returns>Task.</returns>
    Task<T?> GetByIdAsync(object id);

    /// <summary>
    /// Inserts the asynchronous.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<int> InsertAsync(T entity);

    /// <summary>
    /// Updates the asynchronous.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<int> UpdateAsync(T entity);

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<int> DeleteAsync(object id);

    /// <summary>
    /// Asynchronous check on any.
    /// </summary>
    /// <param name="whereClause">The where clause.</param>
    /// <param name="param">The parameter.</param>
    /// <returns>Task.</returns>
    Task<bool> AnyAsync(string whereClause, object? param = null);

    /// <summary>
    /// Async search.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <returns>Task.</returns>
    Task<PagedListResult<T>> SearchAsync(DapperSearchQuery query);
}