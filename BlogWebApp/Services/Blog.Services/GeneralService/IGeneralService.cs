// <copyright file="IGeneralService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.GeneralService;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.Infrastructure.Pagination;
using Blog.Core.TableFilters;
using Data.Specifications.Base;

/// <summary>
/// General service interface.
/// </summary>
/// <typeparam name="T">Type.</typeparam>
public interface IGeneralService<T>
{
    /// <summary>
    /// Find item by id.
    /// </summary>
    /// <param name="id">id.</param>
    /// <returns>Type.</returns>
    T Find(object id);

    /// <summary>
    /// Async find item by id.
    /// </summary>
    /// <param name="id">id.</param>
    /// <returns>Task.</returns>
    Task<T> FindAsync(object id);

    /// <summary>
    /// Insert item.
    /// </summary>
    /// <param name="entity">entity.</param>
    void Insert(T entity);

    /// <summary>
    /// Insert IEnumerable.
    /// </summary>
    /// <param name="entities">entities.</param>
    void Insert(IEnumerable<T> entities);

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
    void Update(T entity);

    /// <summary>
    /// Update IEnumerable.
    /// </summary>
    /// <param name="entities">entities.</param>
    void Update(IEnumerable<T> entities);

    /// <summary>
    /// Update item.
    /// </summary>
    /// <param name="entity">entity.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Update IEnumerable.
    /// </summary>
    /// <param name="entities">entities.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task UpdateAsync(IEnumerable<T> entities);

    /// <summary>
    /// Delete item.
    /// </summary>
    /// <param name="id">entity.</param>
    void Delete(int id);

    /// <summary>
    /// Delete item.
    /// </summary>
    /// <param name="entity">entity.</param>
    void Delete(T entity);

    /// <summary>
    /// Delete IEnumerable.
    /// </summary>
    /// <param name="entities">IEnumerable.</param>
    void Delete(IEnumerable<T> entities);

    /// <summary>
    /// Delete item.
    /// </summary>
    /// <param name="id">entity.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteAsync(int id);

    /// <summary>
    /// Delete item.
    /// </summary>
    /// <param name="entity">entity.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteAsync(T entity);

    /// <summary>
    /// Delete IEnumerable.
    /// </summary>
    /// <param name="entities">IEnumerable.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteAsync(IEnumerable<T> entities);

    /// <summary>
    /// Async search.
    /// </summary>
    /// <param name="searchQuery">searchQuery.</param>
    /// <returns>Task.</returns>
    Task<PagedListResult<T>> SearchAsync(SearchQuery<T> searchQuery);

    /// <summary>
    /// Async Search by sequence.
    /// </summary>
    /// <param name="searchQuery">searchQuery.</param>
    /// <param name="sequence">sequence.</param>
    /// <returns>Task.</returns>
    Task<PagedListResult<T>> SearchBySequenceAsync(
        SearchQuery<T> searchQuery,
        IQueryable<T> sequence);

    /// <summary>
    /// Generate query.
    /// </summary>
    /// <param name="tableFilter">tableFilter.</param>
    /// <param name="includeProperties">includeProperties.</param>
    /// <returns>SearchQuery.</returns>
    SearchQuery<T> GenerateQuery(TableFilter tableFilter, string includeProperties = null);

    /// <summary>
    /// Get all items.
    /// </summary>
    /// <returns>ICollection.</returns>
    ICollection<T> GetAll();

    /// <summary>
    /// Gets all asynchronous.
    /// </summary>
    /// <returns>Task.</returns>
    Task<ICollection<T>> GetAllAsync();

    /// <summary>
    /// Get all items by specification.
    /// </summary>
    /// <param name="specification">The specification.</param>
    /// <returns>ICollection.</returns>
    ICollection<T> GetAll(ISpecification<T> specification);

    /// <summary>
    /// Async get all items by specification.
    /// </summary>
    /// <param name="specification">The specification.</param>
    /// <returns>Task.</returns>
    Task<ICollection<T>> GetAllAsync(ISpecification<T> specification);

    /// <summary>
    /// Check on any by specification.
    /// </summary>
    /// <param name="specification">The specification.</param>
    /// <returns>bool.</returns>
    bool Any(ISpecification<T> specification);

    /// <summary>
    /// Asynchronous check on any by specification.
    /// </summary>
    /// <param name="specification">The specification.</param>
    /// <returns>Task.</returns>
    Task<bool> AnyAsync(ISpecification<T> specification);

    /// <summary>
    /// Get first or default value.
    /// </summary>
    /// <param name="specification">The specification.</param>
    /// <returns>Type.</returns>
    T FirstOrDefault(ISpecification<T> specification);

    /// <summary>
    /// Lasts the or default.
    /// </summary>
    /// <param name="specification">The specification.</param>
    /// <returns>T.</returns>
    T LastOrDefault(ISpecification<T> specification);
}