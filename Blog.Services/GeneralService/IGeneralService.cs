// <copyright file="IGeneralService.cs" company="Blog">
// Copyright (c) BLog. All rights reserved.
// </copyright>

namespace Blog.Services.GeneralService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Blog.Core.Infrastructure.Pagination;
    using Blog.Core.TableFilters;

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
        /// Get all items by expression.
        /// </summary>
        /// <param name="expression">expression.</param>
        /// <returns>ICollection.</returns>
        ICollection<T> GetAll(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task<ICollection<T>> GetAllAsync();

        /// <summary>
        /// Async get all items by expression.
        /// </summary>
        /// <param name="expression">expression.</param>
        /// <returns>Task.</returns>
        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Check on any by expression.
        /// </summary>
        /// <param name="expression">expression.</param>
        /// <returns>bool.</returns>
        bool Any(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Get first or default value.
        /// </summary>
        /// <param name="expression">expression.</param>
        /// <returns>Type.</returns>
        T FirstOrDefault(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Get last or default value.
        /// </summary>
        /// <param name="expression">expression.</param>
        /// <returns>Type.</returns>
        T LastOrDefault(Expression<Func<T, bool>> expression);
    }
}
