// <copyright file="IRepository.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Blog.Core;
    using Blog.Core.Infrastructure.Pagination;
    using Blog.Core.TableFilters;

    /// <summary>
    /// Repository interface.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    public interface IRepository<T>
        where T : IEntity
    {
        /// <summary>
        /// Gets table.
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        /// Gets no tracked table.
        /// </summary>
        IQueryable<T> TableNoTracking { get; }

        /// <summary>
        /// Get all items.
        /// </summary>
        /// <returns>Type.</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Get all items by expressiom.
        /// </summary>
        /// <param name="expression">expression.</param>
        /// <returns>IQueryable.</returns>
        IQueryable<T> GetAll(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Get item by id.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>Type.</returns>
        T GetById(object id);

        /// <summary>
        /// Async get item by id async.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>Task.</returns>
        Task<T> GetByIdAsync(object id);

        /// <summary>
        /// Async search.
        /// </summary>
        /// <param name="searchQuery">searchQuery.</param>
        /// <returns>Task.</returns>
        Task<PagedListResult<T>> SearchAsync(SearchQuery<T> searchQuery);

        /// <summary>
        /// Async search by sequence.
        /// </summary>
        /// <param name="searchQuery">searchQuery.</param>
        /// <param name="sequence">sequence.</param>
        /// <returns>Task.</returns>
        Task<PagedListResult<T>> SearchBySquenceAsync(SearchQuery<T> searchQuery, IQueryable<T> sequence);

        /// <summary>
        /// Generate query.
        /// </summary>
        /// <param name="tableFilter">tableFilter.</param>
        /// <param name="includeProperties">includeProperties.</param>
        /// <returns>SearchQuery.</returns>
        SearchQuery<T> GenerateQuery(TableFilter tableFilter, string includeProperties = null);

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
        /// <param name="entity">The entity.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task InsertAsync(T entity);

        /// <summary>
        /// Inserts the asynchronous.
        /// </summary>
        /// <param name="entities">The entities.</param>
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
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task UpdateAsync(IEnumerable<T> entities);

        /// <summary>
        /// Delete item.
        /// </summary>
        /// <param name="entity">entity.</param>
        void Delete(T entity);

        /// <summary>
        /// Delete IEnumerable.
        /// </summary>
        /// <param name="entities">entities.</param>
        void Delete(IEnumerable<T> entities);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task DeleteAsync(T entity);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task DeleteAsync(IEnumerable<T> entities);

        /// <summary>
        /// Check on any.
        /// </summary>
        /// <param name="expression">expression.</param>
        /// <returns>bool.</returns>
        bool Any(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Asynchronous check on any.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>Task.</returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Get first or default.
        /// </summary>
        /// <param name="expression">expression.</param>
        /// <returns>Type.</returns>
        T FirstOrDefault(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Get last or default.
        /// </summary>
        /// <param name="expression">expression.</param>
        /// <returns>Type.</returns>
        T LastOrDefault(Expression<Func<T, bool>> expression);
    }
}
