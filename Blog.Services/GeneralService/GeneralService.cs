// <copyright file="GeneralService.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
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
    using Blog.Data.Core;
    using Data.Repository;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// General service.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    public class GeneralService<T> : IGeneralService<T>
        where T : Entity
    {
        /// <summary>
        /// Repository.
        /// </summary>
        protected IRepository<T> Repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralService{T}"/> class.
        /// </summary>
        /// <param name="repository">repository.</param>
        public GeneralService(IRepository<T> repository)
        {
            this.Repository = repository;
        }

        /// <summary>
        /// Gets table.
        /// </summary>
        protected IQueryable<T> Table => this.Repository.Table;

        /// <summary>
        /// Gets tableNoTracking.
        /// </summary>
        protected IQueryable<T> TableNoTracking => this.Repository.TableNoTracking;

        /// <inheritdoc/>
        public T Find(object id)
        {
            return this.Repository.GetById(id);
        }

        /// <inheritdoc/>
        public async Task<T> FindAsync(object id)
        {
            return await this.Repository.GetByIdAsync(id);
        }

        /// <inheritdoc/>
        public void Insert(T entity)
        {
            this.Repository.Insert(entity);
        }

        /// <inheritdoc/>
        public void Insert(IEnumerable<T> entities)
        {
            this.Repository.Insert(entities);
        }

        /// <inheritdoc/>
        public async Task InsertAsync(T entity)
        {
            await this.Repository.InsertAsync(entity);
        }

        /// <inheritdoc/>
        public async Task InsertAsync(IEnumerable<T> entities)
        {
            await this.Repository.InsertAsync(entities);
        }

        /// <inheritdoc/>
        public void Update(T entity)
        {
            this.Repository.Update(entity);
        }

        /// <inheritdoc/>
        public void Update(IEnumerable<T> entities)
        {
            this.Repository.Update(entities);
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(T entity)
        {
            await this.Repository.UpdateAsync(entity);
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(IEnumerable<T> entities)
        {
            await this.Repository.UpdateAsync(entities);
        }

        /// <inheritdoc/>
        public void Delete(int id)
        {
            this.Repository.Delete(this.Find(id));
        }

        /// <inheritdoc/>
        public void Delete(T entity)
        {
            this.Repository.Delete(entity);
        }

        /// <inheritdoc/>
        public void Delete(IEnumerable<T> entities)
        {
            this.Repository.Delete(entities);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(int id)
        {
            await this.Repository.DeleteAsync(await this.FindAsync(id));
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(T entity)
        {
            await this.Repository.DeleteAsync(entity);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(IEnumerable<T> entities)
        {
            await this.Repository.DeleteAsync(entities);
        }

        /// <inheritdoc/>
        public async Task<PagedListResult<T>> SearchAsync(SearchQuery<T> searchQuery)
        {
            return await this.Repository.SearchAsync(searchQuery);
        }

        /// <inheritdoc/>
        public async Task<PagedListResult<T>> SearchBySequenceAsync(
            SearchQuery<T> searchQuery,
            IQueryable<T> sequence)
        {
            return await this.Repository.SearchBySquenceAsync(searchQuery, sequence);
        }

        /// <inheritdoc/>
        public ICollection<T> GetAll()
        {
            return this.Repository.GetAll().ToList();
        }

        /// <inheritdoc/>
        public async Task<ICollection<T>> GetAllAsync()
        {
            return await this.Repository.GetAll().ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> expression)
        {
            return await this.Repository.GetAll(expression).ToListAsync();
        }

        /// <inheritdoc/>
        public ICollection<T> GetAll(Expression<Func<T, bool>> expression)
        {
            return this.Repository.GetAll(expression).ToList();
        }

        /// <inheritdoc/>
        public bool Any(Expression<Func<T, bool>> expression)
        {
            return this.Repository.Any(expression);
        }

        /// <inheritdoc/>
        public T FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return this.Repository.FirstOrDefault(expression);
        }

        /// <inheritdoc/>
        public T LastOrDefault(Expression<Func<T, bool>> expression)
        {
            return this.Repository.LastOrDefault(expression);
        }

        /// <inheritdoc/>
        public SearchQuery<T> GenerateQuery(TableFilter tableFilter, string includeProperties = null)
        {
            return this.Repository.GenerateQuery(tableFilter, includeProperties);
        }

        /// <summary>
        /// Get member name.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <typeparam name="TValue">TValue.</typeparam>
        /// <param name="memberAccess">memberAccess.</param>
        /// <returns>string.</returns>
        protected static string GetMemberName<T, TValue>(Expression<Func<T, TValue>> memberAccess)
        {
            return ((MemberExpression)memberAccess.Body).Member.Name;
        }
    }
}
