// <copyright file="GeneralService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Blog.Data.Specifications.Base;

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
    using Blog.Data.Repository;

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
        public IRepository<T> Repository;

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

        /// <inheritdoc cref="IGeneralService{T}"/>
        public T Find(object id)
        {
            return this.Repository.GetById(id);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public async Task<T> FindAsync(object id)
        {
            return await this.Repository.GetByIdAsync(id);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public void Insert(T entity)
        {
            this.Repository.Insert(entity);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public void Insert(IEnumerable<T> entities)
        {
            this.Repository.Insert(entities);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public async Task InsertAsync(T entity)
        {
            await this.Repository.InsertAsync(entity);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public async Task InsertAsync(IEnumerable<T> entities)
        {
            await this.Repository.InsertAsync(entities);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public void Update(T entity)
        {
            this.Repository.Update(entity);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public void Update(IEnumerable<T> entities)
        {
            this.Repository.Update(entities);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public async Task UpdateAsync(T entity)
        {
            await this.Repository.UpdateAsync(entity);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public async Task UpdateAsync(IEnumerable<T> entities)
        {
            await this.Repository.UpdateAsync(entities);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public void Delete(int id)
        {
            this.Repository.Delete(this.Find(id));
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public void Delete(T entity)
        {
            this.Repository.Delete(entity);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public void Delete(IEnumerable<T> entities)
        {
            this.Repository.Delete(entities);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public async Task DeleteAsync(int id)
        {
            await this.Repository.DeleteAsync(await this.FindAsync(id));
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public async Task DeleteAsync(T entity)
        {
            await this.Repository.DeleteAsync(entity);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public async Task DeleteAsync(IEnumerable<T> entities)
        {
            await this.Repository.DeleteAsync(entities);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public async Task<PagedListResult<T>> SearchAsync(SearchQuery<T> searchQuery)
        {
            return await this.Repository.SearchAsync(searchQuery);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public async Task<PagedListResult<T>> SearchBySequenceAsync(
            SearchQuery<T> searchQuery,
            IQueryable<T> sequence)
        {
            return await this.Repository.SearchBySquenceAsync(searchQuery, sequence);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public ICollection<T> GetAll()
        {
            return this.Repository.GetAll().ToList();
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public async Task<ICollection<T>> GetAllAsync()
        {
            return await this.Repository.GetAllAsync();
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public ICollection<T> GetAll(ISpecification<T> specification)
        {
            return this.Repository.GetAll(specification).ToList();
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public async Task<ICollection<T>> GetAllAsync(ISpecification<T> specification)
        {
            return await this.Repository.GetAllAsync(specification);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public bool Any(ISpecification<T> specification)
        {
            return this.Repository.Any(specification);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public async Task<bool> AnyAsync(ISpecification<T> specification)
        {
            return await this.Repository.AnyAsync(specification);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public T FirstOrDefault(ISpecification<T> specification)
        {
            return this.Repository.FirstOrDefault(specification);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
        public T LastOrDefault(ISpecification<T> specification)
        {
            return this.Repository.LastOrDefault(specification);
        }

        /// <inheritdoc cref="IGeneralService{T}"/>
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
