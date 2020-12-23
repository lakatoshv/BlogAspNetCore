// <copyright file="TableMethods.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Blog.Core;
    using Blog.Core.Infrastructure;
    using Blog.Core.Infrastructure.Pagination;
    using Blog.Core.TableFilters;
    using Repository;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Table methods.
    /// </summary>
    /// <typeparam name="TEntity">TEntity.</typeparam>
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Database context.
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Entity.
        /// </summary>
        private DbSet<TEntity> _entities;

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public virtual IQueryable<TEntity> Table => this.Entities;

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public virtual IQueryable<TEntity> TableNoTracking =>

            // AsNoTracking method temporarily doesn't work, it's a bug in EF Core 2.1 (details in https://github.com/aspnet/EntityFrameworkCore/issues/11689)
            // Update - I checked this functionality and it is working fine, that's why I returned
            this.Entities.AsNoTracking();

        /// <summary>
        /// Gets entities.
        /// </summary>
        protected virtual DbSet<TEntity> Entities => this._entities ?? (this._entities = this._context.Set<TEntity>());

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class.
        /// </summary>
        /// <param name="context">context.</param>
        public Repository(ApplicationDbContext context)
        {
            this._context = context;
        }

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public IQueryable<TEntity> GetAll()
        {
            return this.Entities.AsQueryable();
        }

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression)
        {
            return this.Entities.Where(expression);
        }

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public virtual TEntity GetById(object id)
        {
            return this.Entities.Find(id);
        }

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await this.Entities.FindAsync(id);
        }

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public virtual async Task<PagedListResult<TEntity>> SearchAsync(SearchQuery<TEntity> searchQuery)
        {
            IQueryable<TEntity> sequence = this.Entities;

            // Applying filters
            sequence = this.ManageFilters(searchQuery, sequence);

            // Include Properties
            sequence = this.ManageIncludeProperties(searchQuery, sequence);

            // Resolving Sort Criteria
            // This code applies the sorting criterias sent as the parameter
            sequence = this.ManageSortCriterias(searchQuery, sequence);

            return await this.GetTheResult(searchQuery, sequence);
        }

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public virtual async Task<PagedListResult<TEntity>> SearchBySquenceAsync(SearchQuery<TEntity> searchQuery, IQueryable<TEntity> sequence)
        {
            // Applying filters
            sequence = this.ManageFilters(searchQuery, sequence);

            // Include Properties
            sequence = this.ManageIncludeProperties(searchQuery, sequence);

            // Resolving Sort Criteria
            // This code applies the sorting criterias sent as the parameter
            sequence = this.ManageSortCriterias(searchQuery, sequence);

            return await this.GetTheResult(searchQuery, sequence);
        }

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public virtual SearchQuery<TEntity> GenerateQuery(TableFilter tableFilter, string includeProperties = null)
        {
            var query = new SearchQuery<TEntity>
            {
                Skip = tableFilter.Start,
                Take = tableFilter.Length,
            };

            if (!string.IsNullOrEmpty(includeProperties))
            {
                query.IncludeProperties = includeProperties;
            }

            query.AddSortCriteria(new FieldSortOrder<TEntity>(tableFilter.ColumnName, tableFilter.OrderType));

            return query;
        }

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public virtual void Insert(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                this.Entities.Add(entity);
                this._context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                // ensure that the detailed error text is saved in the Log
                throw new Exception(this.GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            try
            {
                this.Entities.AddRange(entities);
                this._context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                // ensure that the detailed error text is saved in the Log
                throw new Exception(this.GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public async Task InsertAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                this.Entities.Add(entity);
                await this._context.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                // ensure that the detailed error text is saved in the Log
                throw new Exception(this.GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            try
            {
                this.Entities.AddRange(entities);
                await this._context.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                // ensure that the detailed error text is saved in the Log
                throw new Exception(this.GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public virtual void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                this.Entities.Update(entity);
                this._context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                // ensure that the detailed error text is saved in the Log
                throw new Exception(this.GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public virtual void Update(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            try
            {
                this.Entities.UpdateRange(entities);
                this._context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                // ensure that the detailed error text is saved in the Log
                throw new Exception(this.GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public async Task UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                this.Entities.Update(entity);
                await this._context.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                // ensure that the detailed error text is saved in the Log
                throw new Exception(this.GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public async Task UpdateAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            try
            {
                this.Entities.UpdateRange(entities);
                await this._context.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                // ensure that the detailed error text is saved in the Log
                throw new Exception(this.GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                this.Entities.Remove(entity);
                this._context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                // ensure that the detailed error text is saved in the Log
                throw new Exception(this.GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            try
            {
                this.Entities.RemoveRange(entities);
                this._context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                // ensure that the detailed error text is saved in the Log
                throw new Exception(this.GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public async Task DeleteAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                this.Entities.Remove(entity);
                await this._context.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                // ensure that the detailed error text is saved in the Log
                throw new Exception(this.GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            try
            {
                this.Entities.RemoveRange(entities);
                await this._context.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                // ensure that the detailed error text is saved in the Log
                throw new Exception(this.GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public bool Any(Expression<Func<TEntity, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return this.Entities.Any(expression);
        }

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return await this.Entities.AnyAsync(expression);
        }

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return this.Entities.FirstOrDefault(expression);
        }

        /// <inheritdoc cref="IRepository{TEntity}"/>
        public TEntity LastOrDefault(Expression<Func<TEntity, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            return this.Entities.LastOrDefault(expression);
        }

        /// <summary>
        /// Get full error text and rollback entity changes.
        /// </summary>
        /// <param name="exception">exception.</param>
        /// <returns>string.</returns>
        protected string GetFullErrorTextAndRollbackEntityChanges(DbUpdateException exception)
        {
            // rollback entity changes
            if (this._context is DbContext dbContext)
            {
                var entries = dbContext.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

                entries.ForEach(entry => entry.State = EntityState.Unchanged);
            }

            this._context.SaveChanges();
            return exception.ToString();
        }

        /// <summary>
        /// Get result.
        /// </summary>
        /// <param name="searchQuery">searchQuery.</param>
        /// <param name="sequence">sequence.</param>
        /// <returns>Task.</returns>
        protected virtual async Task<PagedListResult<TEntity>> GetTheResult(SearchQuery<TEntity> searchQuery, IQueryable<TEntity> sequence)
        {
            // Counting the total number of object.
            var resultCount = await sequence.CountAsync();

            var result = (searchQuery.Take > 0)
                                ? await sequence.Skip(searchQuery.Skip).Take(searchQuery.Take).ToListAsync()
                                : await sequence.ToListAsync();

            // Debug info of what the query looks like
            // Console.WriteLine(sequence.ToString());

            // Setting up the return object.
            bool hasNext = (searchQuery.Skip > 0 || searchQuery.Take > 0) && (searchQuery.Skip + searchQuery.Take < resultCount);
            return new PagedListResult<TEntity>()
            {
                Entities = result,
                HasNext = hasNext,
                HasPrevious = searchQuery.Skip > 0,
                Count = resultCount,
            };
        }

        /// <summary>
        /// Manage sort criterias.
        /// </summary>
        /// <param name="searchQuery">searchQuery.</param>
        /// <param name="sequence">sequence.</param>
        /// <returns>IQueryable.</returns>
        protected virtual IQueryable<TEntity> ManageSortCriterias(SearchQuery<TEntity> searchQuery, IQueryable<TEntity> sequence)
        {
            if (searchQuery.SortCriterias != null && searchQuery.SortCriterias.Count > 0)
            {
                var sortCriteria = searchQuery.SortCriterias[0];
                var orderedSequence = sortCriteria.ApplyOrdering(sequence, false);

                if (searchQuery.SortCriterias.Count > 1)
                {
                    for (var i = 1; i < searchQuery.SortCriterias.Count; i++)
                    {
                        var sc = searchQuery.SortCriterias[i];
                        orderedSequence = sc.ApplyOrdering(orderedSequence, true);
                    }
                }

                sequence = orderedSequence;
            }
            else
            {
                sequence = ((IOrderedQueryable<TEntity>)sequence).OrderBy(x => true);
            }

            return sequence;
        }

        /// <summary>
        /// Manage filters.
        /// </summary>
        /// <param name="searchQuery">searchQuery.</param>
        /// <param name="sequence">sequence.</param>
        /// <returns>IQueryable.</returns>
        protected virtual IQueryable<TEntity> ManageFilters(SearchQuery<TEntity> searchQuery, IQueryable<TEntity> sequence)
        {
            if (searchQuery.Filters != null && searchQuery.Filters.Count > 0)
            {
                foreach (var filterClause in searchQuery.Filters)
                {
                    sequence = sequence.Where(filterClause);
                }
            }

            return sequence;
        }

        /// <summary>
        /// Manage include properties.
        /// </summary>
        /// <param name="searchQuery">searchQuery.</param>
        /// <param name="sequence">sequence.</param>
        /// <returns>IQueryable.</returns>
        protected virtual IQueryable<TEntity> ManageIncludeProperties(SearchQuery<TEntity> searchQuery, IQueryable<TEntity> sequence)
        {
            if (!string.IsNullOrWhiteSpace(searchQuery.IncludeProperties))
            {
                var properties = searchQuery.IncludeProperties.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var includeProperty in properties)
                {
                    sequence = sequence.Include(includeProperty);
                }
            }

            return sequence;
        }
    }
}
