namespace Blog.Data.UnitOfWork
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Blog.Core;
    using Blog.Data.Repository;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.Storage;

    /// <summary>
    /// Unit of work interface.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="hasCustomRepository">if set to <c>true</c> [has custom repository].</param>
        /// <returns>IRepository.</returns>
        IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false)
            where TEntity : IEntity;

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="ensureAutoHistory">if set to <c>true</c> [ensure automatic history].</param>
        /// <returns>int.</returns>
        int SaveChanges(bool ensureAutoHistory = false);

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <param name="ensureAutoHistory">if set to <c>true</c> [ensure automatic history].</param>
        /// <returns>Task.</returns>
        Task<int> SaveChangesAsync(bool ensureAutoHistory = false);

        /// <summary>
        /// Executes the SQL command.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>int.</returns>
        int ExecuteSqlCommand(string sql, params object[] parameters);

        /// <summary>
        /// From the SQL raw.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>IQueryable.</returns>
        IQueryable<TEntity> FromSqlRaw<TEntity>(string sql, params object[] parameters) where TEntity : class;

        /// <summary>
        /// Tracks the graph.
        /// </summary>
        /// <param name="rootEntity">The root entity.</param>
        /// <param name="callback">The callback.</param>
        void TrackGraph(object rootEntity, Action<EntityEntryGraphNode> callback);

        /// <summary>
        /// Begins the transaction asynchronous.
        /// </summary>
        /// <param name="useIfExists">if set to <c>true</c> [use if exists].</param>
        /// <returns>Task.</returns>
        Task<IDbContextTransaction> BeginTransactionAsync(bool useIfExists = false);

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="useIfExists">if set to <c>true</c> [use if exists].</param>
        /// <returns>IDbContextTransaction.</returns>
        IDbContextTransaction BeginTransaction(bool useIfExists = false);

        /// <summary>
        /// Sets the automatic detect changes.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        void SetAutoDetectChanges(bool value);

        /// <summary>
        /// Gets the last save changes result.
        /// </summary>
        /// <value>
        /// The last save changes result.
        /// </value>
        SaveChangesResult LastSaveChangesResult { get; }
    }

    /// <summary>
    /// Unit of work interface.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <seealso cref="IDisposable" />
    public interface IUnitOfWork<TContext> : IUnitOfWork, IDisposable
        where TContext : Microsoft.EntityFrameworkCore.DbContext
    {
        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <value>
        /// The database context.
        /// </value>
        TContext DbContext { get; }

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <param name="ensureAutoHistory">if set to <c>true</c> [ensure automatic history].</param>
        /// <param name="unitOfWorks">The unit of works.</param>
        /// <returns>Task.</returns>
        Task<int> SaveChangesAsync(bool ensureAutoHistory = false, params IUnitOfWork[] unitOfWorks);
    }
}