// <copyright file="GeneralDapperService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Blog.Core;
using Blog.Core.Infrastructure.Pagination;
using Blog.Data.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.EntityServices.GeneralService;

/// <summary>
/// General dapper service.
/// </summary>
/// <typeparam name="T">Type.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="GeneralDapperService{T}"/> class.
/// </remarks>
/// <param name="repository">repository.</param>
public class GeneralDapperService<T>(IDapperRepository<T> repository)
    : IGeneralDapperService<T>
    where T : class, IEntity
{
    /// <inheritdoc cref="IGeneralDapperService{T}"/>
    public async Task<T?> FindAsync(object id)
        => await repository.GetByIdAsync(id);

    /// <inheritdoc cref="IGeneralDapperService{T}"/>
    public async Task InsertAsync(T entity)
        => await repository.InsertAsync(entity);

    /// <inheritdoc cref="IGeneralDapperService{T}"/>
    public async Task InsertAsync(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
            await repository.InsertAsync(entity);
    }

    /// <inheritdoc cref="IGeneralDapperService{T}"/>
    public async Task UpdateAsync(T entity)
        => await repository.UpdateAsync(entity);

    /// <inheritdoc cref="IGeneralDapperService{T}"/>
    public async Task DeleteAsync(object id)
        => await repository.DeleteAsync(id);

    /// <inheritdoc cref="IGeneralDapperService{T}"/>
    public async Task<IEnumerable<T>> GetAllAsync()
        => await repository.GetAllAsync();

    /// <inheritdoc cref="IGeneralDapperService{T}"/>
    public async Task<bool> AnyAsync(string whereClause, object? param = null)
        => await repository.AnyAsync(whereClause, param);

    /// <inheritdoc cref="IGeneralDapperService{T}"/>
    public async Task<PagedListResult<T>> SearchAsync(DapperSearchQuery query)
        => await repository.SearchAsync(query);
}