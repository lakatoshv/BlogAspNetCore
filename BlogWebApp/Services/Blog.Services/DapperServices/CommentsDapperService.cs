// <copyright file="CommentsDapperService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
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
using System.Threading.Tasks;

namespace Blog.EntityServices.DapperServices;

/// <summary>
/// Comments dapper service.
/// </summary>
/// <seealso cref="GeneralDapperService{T}" />
/// <seealso cref="ICommentsDapperService" />
/// <remarks>
/// Initializes a new instance of the <see cref="CommentsDapperService"/> class.
/// </remarks>
/// <param name="repo">The repo.</param>
/// <param name="connection">The connection.</param>
public class CommentsDapperService(IDapperRepository<Comment> repo, IDbConnection connection)
    : GeneralDapperService<Comment>(repo), ICommentsDapperService
{
    /// <inheritdoc cref="ICommentsDapperService"/>
    public async Task<CommentsViewDto> GetPagedCommentsByPostId(int postId, SortParametersDto sortParameters)
    {
        var param = new DynamicParameters();
        param.Add("@PostId", postId);

        const string countSql = "SELECT COUNT(*) FROM Comments WHERE PostId = @PostId";
        var total = await connection.ExecuteScalarAsync<int>(countSql, param);

        const string sql = """
                                   SELECT c.*, u.Id, u.Email, u.FirstName, u.LastName, u.PhoneNumber
                                   FROM Comments c
                                   LEFT JOIN AspNetUsers u ON u.Id = c.UserId
                                   WHERE c.PostId = @PostId
                                   ORDER BY c.CreatedAt DESC
                                   OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY
                           """;

        param.Add("@Skip", (sortParameters.CurrentPage.Value - 1) * sortParameters.PageSize.Value);
        param.Add("@Take", sortParameters.PageSize.Value);

        var comments = await connection.QueryAsync<Comment, ApplicationUser, Comment>(
            sql,
            (c, u) =>
            {
                c.User = u ?? new ApplicationUser();
                return c;
            },
            param,
            splitOn: "Id");

        return new CommentsViewDto
        {
            Comments = comments.ToList(),
            PageInfo = new PageInfo
            {
                PageNumber = sortParameters.CurrentPage.Value,
                PageSize = sortParameters.PageSize.Value,
                TotalItems = total
            }
        };
    }

    /// <inheritdoc cref="ICommentsDapperService"/>
    public async Task<Comment> GetCommentAsync(int id)
    {
        const string sql = """
                                   SELECT c.*, u.Id, u.Email, u.FirstName, u.LastName, u.PhoneNumber
                                   FROM Comments c
                                   LEFT JOIN AspNetUsers u ON u.Id = c.UserId
                                   WHERE c.Id = @Id
                           """;

        return (await connection.QueryAsync<Comment, ApplicationUser, Comment>(
            sql,
            (c, u) =>
            {
                c.User = u;
                return c;
            },
            new { Id = id },
            splitOn: "Id")).FirstOrDefault();
    }

    /// <inheritdoc cref="ICommentsDapperService"/>
    public async Task<CommentsViewDto> GetPagedComments(SortParametersDto sortParameters)
    {
        const string countSql = "SELECT COUNT(*) FROM Comments";
        var total = await connection.ExecuteScalarAsync<int>(countSql);

        if (sortParameters.CurrentPage == null || sortParameters.PageSize == null)
        {
            var all = await connection.QueryAsync<Comment, ApplicationUser, Comment>(
                """
                SELECT c.*, u.Id, u.Email, u.FirstName, u.LastName, u.PhoneNumber
                              FROM Comments c
                              LEFT JOIN AspNetUsers u ON u.Id = c.UserId
                """,
                (c, u) =>
                {
                    c.User = u;
                    return c;
                },
                splitOn: "Id");

            return new CommentsViewDto { Comments = all.ToList() };
        }

        const string sql = """
                                   SELECT c.*, u.Id, u.Email, u.FirstName, u.LastName, u.PhoneNumber
                                   FROM Comments c
                                   LEFT JOIN AspNetUsers u ON u.Id = c.UserId
                                   ORDER BY c.CreatedAt DESC
                                   OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY
                           """;

        var comments = await connection.QueryAsync<Comment, ApplicationUser, Comment>(
            sql,
            (c, u) =>
            {
                c.User = u;
                return c;
            },
            new
            {
                Skip = (sortParameters.CurrentPage.Value - 1) * sortParameters.PageSize.Value,
                Take = sortParameters.PageSize.Value
            },
            splitOn: "Id");

        return new CommentsViewDto
        {
            Comments = comments.ToList(),
            PageInfo = new PageInfo
            {
                PageNumber = sortParameters.CurrentPage.Value,
                PageSize = sortParameters.PageSize.Value,
                TotalItems = total
            }
        };
    }

    /// <inheritdoc cref="ICommentsDapperService"/>
    public async Task<ChartDataModel> GetCommentsActivity()
    {
        const string sql = """
                                   SELECT CONVERT(date, CreatedAt) Date, COUNT(*) Count
                                   FROM Comments
                                   GROUP BY CONVERT(date, CreatedAt)
                                   ORDER BY Date
                           """;

        var data = await connection.QueryAsync(sql);

        return new ChartDataModel
        {
            Name = "Comments",
            Series = data.Select(x => new ChartItem
            {
                Name = ((DateTime)x.Date).ToString("dd/MM/yyyy"),
                Value = (int)x.Count
            }).ToList()
        };
    }
}