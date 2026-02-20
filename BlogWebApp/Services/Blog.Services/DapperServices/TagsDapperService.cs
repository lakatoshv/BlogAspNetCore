// <copyright file="TagsDapperService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Blog.Contracts.V1.Responses.Chart;
using Blog.Core.Helpers;
using Blog.Data.Models;
using Blog.Data.Repository;
using Blog.EntityServices.DapperServices.Interfaces;
using Blog.EntityServices.GeneralService;
using Blog.Services.Core.Dtos;
using Blog.Services.Core.Dtos.Posts;
using Dapper;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.EntityServices.DapperServices;

/// <summary>
/// Tags service.
/// </summary>
/// <seealso cref="GeneralDapperService{Tag}" />
/// <seealso cref="ITagsDapperService" />
/// <remarks>
/// Initializes a new instance of the <see cref="TagsDapperService"/> class.
/// </remarks>
/// <param name="repo">The repo.</param>
/// <param name="connection">The connection.</param>
public class TagsDapperService(IDapperRepository<Tag> repo,
    IDbConnection connection)
    : GeneralDapperService<Tag>(repo), ITagsDapperService
{
    /// <inheritdoc cref="ICommentsDapperService"/>
    public async Task<TagsViewDto> GetTagsAsync(SearchParametersDto searchParameters)
    {
        var result = new TagsViewDto();

        var sqlBuilder = new StringBuilder();
        var countSqlBuilder = new StringBuilder();

        sqlBuilder.Append("SELECT Id, Title FROM Tags WHERE 1=1 ");
        countSqlBuilder.Append("SELECT COUNT(*) FROM Tags WHERE 1=1 ");

        var parameters = new DynamicParameters();

        // Search
        if (!string.IsNullOrWhiteSpace(searchParameters.Search))
        {
            sqlBuilder.Append(" AND LOWER(Title) LIKE @Search ");
            countSqlBuilder.Append(" AND LOWER(Title) LIKE @Search ");
            parameters.Add("Search", $"%{searchParameters.Search.ToLower()}%");
        }

        // Sorting
        if (searchParameters.SortParameters?.SortBy != null)
        {
            sqlBuilder.Append($" ORDER BY {searchParameters.SortParameters.OrderBy} ");

            sqlBuilder.Append(searchParameters.SortParameters.SortBy == "desc" ? " DESC " : " ASC ");
        }
        else
        {
            sqlBuilder.Append(" ORDER BY Id ASC ");
        }

        // Pagination
        if (searchParameters.SortParameters is { CurrentPage: not null, PageSize: not null })
        {
            sqlBuilder.Append(" OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY ");

            parameters.Add("Offset",
                (searchParameters.SortParameters.CurrentPage.Value - 1) *
                searchParameters.SortParameters.PageSize.Value);

            parameters.Add("PageSize",
                searchParameters.SortParameters.PageSize.Value);
        }

        // Execute
        var tags = await connection.QueryAsync<TagViewDto>(
            sqlBuilder.ToString(),
            parameters);

        var totalCount = await connection.ExecuteScalarAsync<int>(
            countSqlBuilder.ToString(),
            parameters);

        result.Tags = tags.ToList();

        if (searchParameters.SortParameters is { CurrentPage: not null, PageSize: not null })
        {
            result.PageInfo = new PageInfo
            {
                PageNumber = searchParameters.SortParameters.CurrentPage.Value,
                PageSize = searchParameters.SortParameters.PageSize.Value,
                TotalItems = totalCount
            };
        }

        return result;
    }

    /// <inheritdoc cref="ICommentsDapperService"/>
    public async Task<ChartDataModel> GetTagsActivity()
    {
        const string sql = """
                                   SELECT Title AS Name,
                                          COUNT(*) AS Value
                                   FROM Tags
                                   GROUP BY Title
                                   ORDER BY COUNT(*) DESC
                           """;

        var series = await connection.QueryAsync<ChartItem>(sql);

        return new ChartDataModel
        {
            Name = "Posts",
            Series = series.ToList()
        };
    }
}