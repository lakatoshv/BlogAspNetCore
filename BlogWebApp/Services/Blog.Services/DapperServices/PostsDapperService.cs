// <copyright file="PostsDapperService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Blog.Contracts.V1.Responses.Chart;
using Blog.Core.Helpers;
using Blog.Data.Models;
using Blog.Data.Repository;
using Blog.EntityServices.DapperServices.Interfaces;
using Blog.EntityServices.GeneralService;
using Blog.Services.Core.Dtos;
using Blog.Services.Core.Dtos.Exports;
using Blog.Services.Core.Dtos.Posts;
using Blog.Services.Core.Dtos.User;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Profile = Blog.Data.Models.Profile;

namespace Blog.EntityServices.DapperServices;

/// <summary>
/// Posts dapper service.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="PostsDapperService"/> class.
/// </remarks>
/// <param name="postsRepo">The postsRepo.</param>
/// <param name="commentsDapperService">The comments dapper service.</param>
/// <param name="connection">The connection.</param>
public class PostsDapperService(
    IDapperRepository<Post> postsRepo,
    ICommentsDapperService commentsDapperService,
    IDbConnection connection)
    : GeneralDapperService<Post>(postsRepo), IPostsDapperService
{
    /// <inheritdoc cref="ICommentsDapperService"/>
    public async Task<PostsViewDto> GetPostsAsync(PostsSearchParametersDto searchParameters)
    {
        var where = "WHERE 1=1";
        var param = new DynamicParameters();

        if (!string.IsNullOrEmpty(searchParameters.Search))
        {
            where += " AND p.Title LIKE @Search";
            param.Add("@Search", $"%{searchParameters.Search}%");
        }

        if (!string.IsNullOrEmpty(searchParameters.Tag))
        {
            where += " AND EXISTS (SELECT 1 FROM PostsTagsRelations r JOIN Tags t ON t.Id=r.TagId WHERE r.PostId=p.Id AND t.Title=@Tag)";
            param.Add("@Tag", searchParameters.Tag);
        }

        var countSql = $"SELECT COUNT(*) FROM Posts p {where}";
        var total = await connection.ExecuteScalarAsync<int>(countSql, param);

        var sql = $"""
                           SELECT p.*, u.*, COUNT(c.Id) CommentsCount
                           FROM Posts p
                           LEFT JOIN AspNetUsers u ON u.Id = p.AuthorId
                           LEFT JOIN Comments c ON c.PostId = p.Id
                           {where}
                           GROUP BY p.Id, u.Id
                           ORDER BY p.CreatedAt DESC
                           OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY
                   """;

        param.Add("@Skip", (searchParameters.SortParameters.CurrentPage - 1) * searchParameters.SortParameters.PageSize);
        param.Add("@Take", searchParameters.SortParameters.PageSize);

        var posts = await connection.QueryAsync<PostViewDto, ApplicationUserDto, PostViewDto>(
            sql,
            (post, author) =>
            {
                post.Author = author;
                return post;
            },
            param,
            splitOn: "Id"
        );

        return new PostsViewDto
        {
            Posts = posts.ToList(),
            PageInfo = new PageInfo
            {
                PageNumber = searchParameters.SortParameters.CurrentPage.Value,
                PageSize = searchParameters.SortParameters.PageSize.Value,
                TotalItems = total
            }
        };
    }

    /// <inheritdoc cref="ICommentsDapperService"/>
    public async Task<Post> GetPostAsync(int id)
    {
        const string sql = """
                                   SELECT p.*, 
                                          u.*, 
                                          pr.*, 
                                          t.*, 
                                          ptr.*
                                   FROM Posts p
                                   LEFT JOIN AspNetUsers u ON u.Id = p.AuthorId
                                   LEFT JOIN Profiles pr ON pr.UserId = u.Id
                                   LEFT JOIN PostsTagsRelations ptr ON ptr.PostId = p.Id
                                   LEFT JOIN Tags t ON t.Id = ptr.TagId
                                   WHERE p.Id = @Id
                           """;

        var postDict = new Dictionary<int, Post>();

        var result = await connection.QueryAsync<Post, ApplicationUser, Profile, Tag, PostsTagsRelations, Post>(
            sql,
            (post, user, profile, tag, relation) =>
            {
                if (!postDict.TryGetValue(post.Id, out var currentPost))
                {
                    currentPost = post;
                    currentPost.Author = user;
                    currentPost.Author.Profile = profile;
                    currentPost.PostsTagsRelations = new List<PostsTagsRelations>();
                    postDict.Add(post.Id, currentPost);
                }

                if (relation == null)
                {
                    return currentPost;
                }

                relation.Tag = tag;
                currentPost.PostsTagsRelations.Add(relation);

                return currentPost;
            },
            new { Id = id },
            splitOn: "Id,Id,Id,Id"
        );

        return postDict.Values.FirstOrDefault();
    }

    /// <inheritdoc cref="ICommentsDapperService"/>
    public async Task<PostShowViewDto> GetPost(int postId, SortParametersDto sortParameters)
    {
        const string sql = """
                                   SELECT 
                                       p.Id, p.Title, p.Description, p.Content, p.Seen, p.Likes, p.Dislikes, p.ImageUrl,
                                       u.Id, u.FirstName, u.LastName, u.Email,
                                       t.Id, t.Title
                                   FROM Posts p
                                   LEFT JOIN AspNetUsers u ON u.Id = p.AuthorId
                                   LEFT JOIN PostsTagsRelations ptr ON ptr.PostId = p.Id
                                   LEFT JOIN Tags t ON t.Id = ptr.TagId
                                   WHERE p.Id = @Id
                           """;

        var postDict = new Dictionary<int, PostShowViewDto>();

        await connection.QueryAsync<PostViewDto, ApplicationUserDto, TagViewDto, PostShowViewDto>(
            sql,
            (post, author, tag) =>
            {
                if (!postDict.TryGetValue(post.Id, out var dto))
                {
                    dto = new PostShowViewDto
                    {
                        Post = post,
                        Tags = new List<TagViewDto>()
                    };
                    dto.Post.Author = author;
                    postDict.Add(post.Id, dto);
                }

                if (tag != null)
                    dto.Tags.Add(tag);

                return dto;
            },
            new { Id = postId },
            splitOn: "Id,Id"
        );

        var model = postDict.Values.FirstOrDefault();
        model.Comments = await commentsDapperService.GetPagedCommentsByPostId(postId, sortParameters);

        return model;
    }

    /// <inheritdoc cref="ICommentsDapperService"/>
    public async Task<PostsViewDto> GetUserPostsAsync(string userId, PostsSearchParametersDto searchParameters)
    {
        var where = "WHERE p.AuthorId = @UserId";
        var param = new DynamicParameters(new { UserId = userId });

        if (!string.IsNullOrWhiteSpace(searchParameters.Search))
        {
            where += " AND LOWER(p.Title) LIKE LOWER(@Search)";
            param.Add("@Search", $"%{searchParameters.Search}%");
        }

        if (!string.IsNullOrWhiteSpace(searchParameters.Tag))
        {
            where += """
                      AND EXISTS (
                                         SELECT 1 FROM PostsTagsRelations ptr
                                         JOIN Tags t ON t.Id = ptr.TagId
                                         WHERE ptr.PostId = p.Id AND LOWER(t.Title) = LOWER(@Tag)
                                       )
                     """;
            param.Add("@Tag", searchParameters.Tag);
        }

        var countSql = $"SELECT COUNT(*) FROM Posts p {where}";
        var total = await connection.ExecuteScalarAsync<int>(countSql, param);

        var sql = $"""
                               SELECT p.*, 
                                      u.Id, u.Email, u.FirstName, u.LastName,
                                      COUNT(DISTINCT c.Id) CommentsCount
                               FROM Posts p
                               LEFT JOIN AspNetUsers u ON u.Id = p.AuthorId
                               LEFT JOIN Comments c ON c.PostId = p.Id
                               {where}
                               GROUP BY p.Id, u.Id, u.Email, u.FirstName, u.LastName
                               ORDER BY p.CreatedAt DESC
                       OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY
                   """;

        param.Add("@Skip", (searchParameters.SortParameters.CurrentPage.Value - 1) * searchParameters.SortParameters.PageSize.Value);
        param.Add("@Take", searchParameters.SortParameters.PageSize.Value);

        var posts = await connection.QueryAsync<PostViewDto, ApplicationUserDto, PostViewDto>(
            sql,
            (p, u) =>
            {
                p.Author = u;
                return p;
            },
            param,
            splitOn: "Id");

        return new PostsViewDto
        {
            Posts = posts.ToList(),
            PageInfo = new PageInfo
            {
                PageNumber = searchParameters.SortParameters.CurrentPage.Value,
                PageSize = searchParameters.SortParameters.PageSize.Value,
                TotalItems = total
            }
        };
    }

    /// <inheritdoc cref="ICommentsDapperService"/>
    public async Task InsertAsync(Post post, IEnumerable<Tag> tags)
    {
        using var transaction = connection.BeginTransaction();

        try
        {
            // INSERT POST and get ID
            var postId = await connection.ExecuteScalarAsync<int>(@"
            INSERT INTO Posts (Title, Description, Content, ImageUrl, AuthorId, CreatedAt)
            VALUES (@Title, @Description, @Content, @ImageUrl, @AuthorId, GETUTCDATE());
            SELECT CAST(SCOPE_IDENTITY() as int);",
                new
                {
                    post.Title,
                    post.Description,
                    post.Content,
                    post.ImageUrl,
                    post.AuthorId
                },
                transaction);

            // Add tags
            foreach (var tag in tags)
            {
                var tagId = await connection.ExecuteScalarAsync<int?>(@"
                SELECT Id FROM Tags WHERE Title = @Title",
                    new { tag.Title },
                    transaction) ?? await connection.ExecuteScalarAsync<int>(@"
                    INSERT INTO Tags (Title)
                    VALUES (@Title);
                    SELECT CAST(SCOPE_IDENTITY() as int);",
                    new { tag.Title },
                    transaction);

                // Create post tag relation
                await connection.ExecuteAsync(@"
                INSERT INTO PostsTagsRelations (PostId, TagId)
                VALUES (@PostId, @TagId)",
                    new { PostId = postId, TagId = tagId },
                    transaction);
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    /// <inheritdoc cref="ICommentsDapperService"/>
    public async Task<ChartDataModel> GetPostsActivity()
    {
        const string sql = """
                                   SELECT CONVERT(date, CreatedAt) Date, COUNT(*) Count
                                   FROM Posts
                                   GROUP BY CONVERT(date, CreatedAt)
                                   ORDER BY Date
                           """;

        var data = await connection.QueryAsync(sql);

        return new ChartDataModel
        {
            Name = "Posts",
            Series = data.Select(x => new ChartItem
            {
                Name = ((DateTime)x.Date).ToString("dd/MM/yyyy"),
                Value = (int)x.Count
            }).ToList()
        };
    }

    /// <inheritdoc cref="ICommentsDapperService"/>
    public async Task<byte[]> ExportPostsToExcel(PostsSearchParametersDto searchParameters)
    {
        try
        {
            var sql = """
                              SELECT 
                                  p.Title,
                                  p.Description,
                                  p.Content,
                                  CONCAT(u.FirstName, ' ', u.LastName, ' (', u.Email, ')') AS Author,
                                  p.Seen,
                                  p.Likes,
                                  p.Dislikes,
                                  p.ImageUrl,
                                  ISNULL(STRING_AGG(t.Title, ', '), '') AS Tags,
                                  COUNT(DISTINCT c.Id) AS CommentsCount
                              FROM Posts p
                              LEFT JOIN AspNetUsers u ON u.Id = p.AuthorId
                              LEFT JOIN Comments c ON c.PostId = p.Id
                              LEFT JOIN PostsTagsRelations ptr ON ptr.PostId = p.Id
                              LEFT JOIN Tags t ON t.Id = ptr.TagId
                              WHERE 1 = 1
                              
                      """;

            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(searchParameters.Search))
            {
                sql += " AND LOWER(p.Title) LIKE LOWER(@Search)";
                parameters.Add("@Search", $"%{searchParameters.Search}%");
            }

            if (!string.IsNullOrWhiteSpace(searchParameters.Tag))
            {
                sql += " AND LOWER(t.Title) = LOWER(@Tag)";
                parameters.Add("@Tag", searchParameters.Tag);
            }

            sql += """
                           GROUP BY 
                               p.Id, p.Title, p.Description, p.Content,
                               u.FirstName, u.LastName, u.Email,
                               p.Seen, p.Likes, p.Dislikes, p.ImageUrl
                   """;

            var rows = await connection.QueryAsync<PostExportRowDto>(sql, parameters);

            var exportRequest = new ExportDataIntoExcelDto
            {
                Headers =
                [
                    new("Title"),
                    new("Description"),
                    new("Content"),
                    new("Author"),
                    new("Seen"),
                    new("Likes"),
                    new("Dislikes"),
                    new("ImageUrl"),
                    new("Tags"),
                    new("Comments count")
                ],
                Rows = []
            };

            foreach (var post in rows)
            {
                var dataTable = new DataTable();
                var row = dataTable.NewRow();

                row["Title"] = post.Title;
                row["Description"] = post.Description;
                row["Content"] = post.Content;
                row["Author"] = post.Author;
                row["Seen"] = post.Seen;
                row["Likes"] = post.Likes;
                row["Dislikes"] = post.Dislikes;
                row["ImageUrl"] = post.ImageUrl;
                row["Tags"] = post.Tags;
                row["Comments count"] = post.CommentsCount;

                exportRequest.Rows.Add(row);
            }

            return null; // exportsService.ExportDataIntoExcel(exportRequest);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}