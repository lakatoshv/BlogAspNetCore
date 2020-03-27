// <copyright file="IDeletableEntityRepository.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Data.Repository
{
    using System.Linq;
    using System.Threading.Tasks;
    using Blog.Core;
    using Blog.Data.Core.Models.Interfaces;

    /// <summary>
    /// Deletable entity repository interface.
    /// </summary>
    /// <typeparam name="TEntity">TEntity.</typeparam>
    public interface IDeletableEntityRepository<TEntity> : IRepository<TEntity>
        where TEntity : IEntity, IDeletableEntity
    {
        /// <summary>
        /// All with deleted.
        /// </summary>
        /// <returns>IQueryable.</returns>
        IQueryable<TEntity> AllWithDeleted();

        /// <summary>
        /// All as no tracking with deleted.
        /// </summary>
        /// <returns>IQueryable.</returns>
        IQueryable<TEntity> AllAsNoTrackingWithDeleted();

        /// <summary>
        /// Get by id with deleted async.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>Task.<TEntity>.</returns>
        Task<TEntity> GetByIdWithDeletedAsync(params object[] id);

        /// <summary>
        /// Hard delete.
        /// </summary>
        /// <param name="entity">entity.</param>
        void HardDelete(TEntity entity);

        /// <summary>
        /// Undelete.
        /// </summary>
        /// <param name="entity">entity.</param>
        void Undelete(TEntity entity);
    }
}
