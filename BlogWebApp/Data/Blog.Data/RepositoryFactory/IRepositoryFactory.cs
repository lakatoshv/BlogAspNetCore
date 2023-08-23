// <copyright file="IRepositoryFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Data.RepositoryFactory;

using Blog.Core;
using Repository;

/// <summary>
/// Repository factory interface.
/// </summary>
public interface IRepositoryFactory
{
    /// <summary>
    /// Gets the repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="hasCustomRepository">if set to <c>true</c> [has custom repository].</param>
    /// <returns>IRepository.</returns>
    IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false)
        where TEntity : IEntity;
}