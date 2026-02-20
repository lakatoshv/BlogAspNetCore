// <copyright file="DapperRepository.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

using Blog.Core;
using Blog.Core.Infrastructure.Pagination;
using Blog.Data.Extensions;
using Blog.Data.Repository;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Data;

/// <summary>
/// Dapper repository.
/// </summary>
/// <typeparam name="T">IEntity.</typeparam>
public class DapperRepository<T>(IDbConnection connection)
    : IDapperRepository<T>
        where T : class, IEntity
{
    /// <summary>
    /// The table name.
    /// </summary>
    private readonly string _tableName = typeof(T).Name;

    /// <inheritdoc cref="IRepository{TEntity}"/>
    public async Task<IEnumerable<T>> GetAllAsync()
        => await connection.QueryAsync<T>($"SELECT * FROM {_tableName}");

    /// <inheritdoc cref="IRepository{TEntity}"/>
    public async Task<T?> GetByIdAsync(object id)
        => await connection.QueryFirstOrDefaultAsync<T>(
            $"SELECT * FROM {_tableName} WHERE Id = @Id", new { Id = id });

    /// <inheritdoc cref="IRepository{TEntity}"/>
    public async Task<int> InsertAsync(T entity)
    {
        var insertQuery = DapperSqlGenerator.GenerateInsert(entity, _tableName);

        return await connection.ExecuteAsync(insertQuery.Sql, insertQuery.Params);
    }

    /// <inheritdoc cref="IRepository{TEntity}"/>
    public async Task<int> UpdateAsync(T entity)
    {
        var updateQuery = DapperSqlGenerator.GenerateUpdate(entity, _tableName);

        return await connection.ExecuteAsync(updateQuery.Sql, updateQuery.Params);
    }

    /// <inheritdoc cref="IRepository{TEntity}"/>
    public async Task<int> DeleteAsync(object id)
        => await connection.ExecuteAsync(
            $"DELETE FROM {_tableName} WHERE Id = @Id", new { Id = id });

    /// <inheritdoc cref="IRepository{TEntity}"/>
    public async Task<bool> AnyAsync(string whereClause, object? param = null)
        => await connection.ExecuteScalarAsync<int>(
            $"SELECT COUNT(1) FROM {_tableName} WHERE {whereClause}", param) > 0;

    /// <inheritdoc cref="IRepository{TEntity}"/>
    public async Task<PagedListResult<T>> SearchAsync(DapperSearchQuery query)
    {
        var where = string.IsNullOrWhiteSpace(query.WhereClause) ? string.Empty : $"WHERE {query.WhereClause}";
        var order = string.IsNullOrWhiteSpace(query.OrderBy) ? string.Empty : $"ORDER BY {query.OrderBy}";

        var sqlCount = $"SELECT COUNT(1) FROM {_tableName} {where}";
        var total = await connection.ExecuteScalarAsync<int>(sqlCount, query.Parameters);

        var sql = $"""
                               SELECT * FROM {_tableName}
                               {where}
                               {order}
                               OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY
                   """;

        var data = await connection.QueryAsync<T>(sql,
            new { query.Skip, query.Take, query.Parameters });

        return new PagedListResult<T>
        {
            Entities = data.ToList(),
            Count = total,
            HasNext = query.Skip + query.Take < total,
            HasPrevious = query.Skip > 0
        };
    }
}