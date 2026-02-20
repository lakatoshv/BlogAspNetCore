// <copyright file="DapperSqlGenerator.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

using System.Linq;

namespace Blog.Data.Extensions;

/// <summary>
/// Dapper Sql generator.
/// </summary>s
public static class DapperSqlGenerator
{
    /// <summary>
    /// Generate insert.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="table">The table.</param>
    /// <returns>The Sql string.</returns>
    /// <returns>The params.</returns>
    public static (string Sql, object Params) GenerateInsert<T>(T entity, string table)
    {
        var props = typeof(T).GetProperties()
            .Where(p => p.Name != "Id");

        var columns = string.Join(",", props.Select(p => p.Name));
        var values = string.Join(",", props.Select(p => "@" + p.Name));

        var sql = $"INSERT INTO {table} ({columns}) VALUES ({values})";

        return (sql, entity);
    }

    /// <summary>
    /// Generate update.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="table">The table.</param>
    /// <returns>The Sql string.</returns>
    /// <returns>The params.</returns>
    public static (string Sql, object Params) GenerateUpdate<T>(T entity, string table)
    {
        var props = typeof(T).GetProperties()
            .Where(p => p.Name != "Id");

        var setClause = string.Join(",", props.Select(p => $"{p.Name} = @{p.Name}"));

        var sql = $"UPDATE {table} SET {setClause} WHERE Id = @Id";

        return (sql, entity);
    }
}