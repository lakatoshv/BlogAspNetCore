namespace Blog.Data.UnitOfWork
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Blog.Core;
    using Blog.Data.Repository;
    using Blog.Data.RepositoryFactory;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Storage;

    /// <summary>
    /// Unit of work.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <seealso cref="IRepositoryFactory" />
    /// <seealso cref="IUnitOfWork{TContext}" />
    /// <seealso cref="IUnitOfWork" />
    /// <seealso cref="IDisposable" />
    public sealed class UnitOfWork<TContext> :
    IRepositoryFactory,
    IUnitOfWork<TContext>,
    IUnitOfWork,
    IDisposable
    where TContext : DbContext
    {
        /// <summary>
        /// The disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// The repositories.
        /// </summary>
        private Dictionary<Type, object> _repositories;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork{TContext}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="ArgumentNullException">context</exception>
        public UnitOfWork(TContext context)
        {
            var context1 = context;
            this.DbContext = (object)context1 != null ? context1 : throw new ArgumentNullException(nameof(context));
            this.LastSaveChangesResult = new SaveChangesResult();
        }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <value>
        /// The database context.
        /// </value>
        public TContext DbContext { get; }

        /// <summary>
        /// Begins the transaction asynchronous.
        /// </summary>
        /// <param name="useIfExists">if set to <c>true</c> [use if exists].</param>
        /// <returns>
        /// Task.
        /// </returns>
        public Task<IDbContextTransaction> BeginTransactionAsync(
          bool useIfExists = false)
        {
            var currentTransaction = this.DbContext.Database.CurrentTransaction;
            if (currentTransaction == null)
            {
                return this.DbContext.Database.BeginTransactionAsync();
            }

            return !useIfExists ? this.DbContext.Database.BeginTransactionAsync() : Task.FromResult(currentTransaction);
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="useIfExists">if set to <c>true</c> [use if exists].</param>
        /// <returns>
        /// IDbContextTransaction.
        /// </returns>
        public IDbContextTransaction BeginTransaction(bool useIfExists = false)
        {
            var currentTransaction = this.DbContext.Database.CurrentTransaction;
            if (currentTransaction == null)
            {
                return this.DbContext.Database.BeginTransaction();
            }

            return !useIfExists ? this.DbContext.Database.BeginTransaction() : currentTransaction;
        }

        /// <summary>
        /// Sets the automatic detect changes.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public void SetAutoDetectChanges(bool value) => this.DbContext.ChangeTracker.AutoDetectChangesEnabled = value;

        /// <summary>
        /// Gets the last save changes result.
        /// </summary>
        /// <value>
        /// The last save changes result.
        /// </value>
        public SaveChangesResult LastSaveChangesResult { get; private set; }

        /// <summary>
        /// Executes the SQL command.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// int.
        /// </returns>
        public int ExecuteSqlCommand(string sql, params object[] parameters)
            => RelationalDatabaseFacadeExtensions.ExecuteSqlRaw(this.DbContext.Database, sql, parameters);

        /// <summary>
        /// From the SQL raw.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// IQueryable.
        /// </returns>
        public IQueryable<TEntity> FromSqlRaw<TEntity>(string sql, params object[] parameters)
            where TEntity : class
            => this.DbContext.Set<TEntity>().FromSqlRaw<TEntity>(sql, parameters);

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="ensureAutoHistory">if set to <c>true</c> [ensure automatic history].</param>
        /// <returns>
        /// int.
        /// </returns>
        public int SaveChanges(bool ensureAutoHistory = false)
        {
            try
            {
                if (ensureAutoHistory)
                {
                    this.DbContext.EnsureAutoHistory();
                }

                return this.DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                this.LastSaveChangesResult.Exception = ex;
                return 0;
            }
        }

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <param name="ensureAutoHistory">if set to <c>true</c> [ensure automatic history].</param>
        /// <returns>
        /// Task.
        /// </returns>
        public async Task<int> SaveChangesAsync(bool ensureAutoHistory = false)
        {
            try
            {
                if (ensureAutoHistory)
                {
                    this.DbContext.EnsureAutoHistory();
                }

                return await this.DbContext.SaveChangesAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                this.LastSaveChangesResult.Exception = ex;
                return 0;
            }
        }

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <param name="ensureAutoHistory">if set to <c>true</c> [ensure automatic history].</param>
        /// <param name="unitOfWorks">The unit of works.</param>
        /// <returns>
        /// Task.
        /// </returns>
        public async Task<int> SaveChangesAsync(
          bool ensureAutoHistory = false,
          params IUnitOfWork[] unitOfWorks)
        {
            var num1 = 0;
            var unitOfWorkArray = unitOfWorks;
            int index;
            for (index = 0; index < unitOfWorkArray.Length; ++index)
            {
                var unitOfWork = unitOfWorkArray[index];
                var num = num1;
                var num2 = ensureAutoHistory ? 1 : 0;
                num1 = num + await unitOfWork.SaveChangesAsync(num2 != 0);
            }

            index = num1;

            return index + await this.SaveChangesAsync(ensureAutoHistory);
        }

        /// <inheritdoc cref="IDisposable"/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        /// <summary>
        /// Tracks the graph.
        /// </summary>
        /// <param name="rootEntity">The root entity.</param>
        /// <param name="callback">The callback.</param>
        public void TrackGraph(object rootEntity, Action<EntityEntryGraphNode> callback) => this.DbContext.ChangeTracker.TrackGraph(rootEntity, callback);

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="hasCustomRepository">if set to <c>true</c> [has custom repository].</param>
        /// <returns>
        /// IRepository.
        /// </returns>
        public IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository) where TEntity : IEntity
        {
            this._repositories ??= new Dictionary<Type, object>();

            if (hasCustomRepository)
            {
                var service = this.DbContext.GetService<IRepository<TEntity>>();
                if (service != null)
                {
                    return service;
                }
            }

            var key = typeof(TEntity);
            if (this._repositories.ContainsKey(key))
            {
                return (IRepository<TEntity>) this._repositories[key];
            }

            var dbContext = this.DbContext;
            if (dbContext != null)
            {
                this._repositories[key] = (object) new Repository<IEntity>((DbContext)dbContext);
            }

            return (IRepository<TEntity>)this._repositories[key];
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (!this._disposed && disposing)
            {
                this._repositories?.Clear();
                this.DbContext.Dispose();
            }

            this._disposed = true;
        }
    }
}