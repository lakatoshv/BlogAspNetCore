// <copyright file="PostsTagsRelationsDapperService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Blog.Data.Models;
using Blog.Data.Repository;
using Blog.EntityServices.DapperServices.Interfaces;
using Blog.EntityServices.GeneralService;
using Blog.EntityServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Blog.EntityServices.DapperServices;

/// <summary>
/// Post tag relations dapper service.
/// </summary>
/// <seealso cref="GeneralDapperService{PostsTagsRelations}" />
/// <seealso cref="IPostsTagsRelationsService" />
/// <remarks>
/// Initializes a new instance of the <see cref="PostsTagsRelationsDapperService"/> class.
/// </remarks>
/// <param name="postsTagsRelationsDapperRepository">The post tag relations dapperR repository.</param>
/// <param name="connection">The connection.</param>
public class PostsTagsRelationsDapperService(
    IDapperRepository<PostsTagsRelations> postsTagsRelationsDapperRepository,
    IDbConnection connection)
    : GeneralDapperService<PostsTagsRelations>(postsTagsRelationsDapperRepository), IPostsTagsRelationsDapperService
{
    /// <inheritdoc cref="ICommentsDapperService"/>
    public async Task AddTagsToPost(int postId, List<PostsTagsRelations> postsTagsRelations, IEnumerable<Tag> tags)
    {
        using var transaction = connection.BeginTransaction();

        try
        {
            var normalizedTitles = tags
                .Where(t => !string.IsNullOrWhiteSpace(t.Title))
                .Select(t => t.Title.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (normalizedTitles.Count == 0)
            {
                return;
            }

            // 1️⃣ Отримати всі існуючі теги одним запитом
            var existingTags = (await connection.QueryAsync<Tag>(
                """
                            SELECT Id, Title 
                              FROM Tags 
                              WHERE Title IN @Titles
                """,
                new { Titles = normalizedTitles },
                transaction)).ToList();

            var existingLookup = existingTags
                .ToDictionary(x => x.Title, StringComparer.OrdinalIgnoreCase);

            // Check new tags
            var newTitles = normalizedTitles
                .Where(title => !existingLookup.ContainsKey(title))
                .ToList();

            // Batch insert new tags
            if (newTitles.Count != 0)
            {
                var insertedTags = (await connection.QueryAsync<Tag>(
                    """
                                    INSERT INTO Tags (Title)
                                      OUTPUT INSERTED.Id, INSERTED.Title
                                      VALUES (@Title)
                    """,
                    newTitles.Select(title => new { Title = title }),
                    transaction)).ToList();

                foreach (var tag in insertedTags)
                    existingLookup[tag.Title] = tag;
            }

            // Get existing relations
            var existingRelations = await connection.QueryAsync<int>(
                """
                            SELECT TagId 
                              FROM PostsTagsRelations 
                              WHERE PostId = @PostId
                """,
                new { PostId = postId },
                transaction);

            var existingRelationSet = new HashSet<int>(existingRelations);

            // Prepare new relations
            var relationsToInsert = existingLookup.Values
                .Where(tag => !existingRelationSet.Contains(tag.Id))
                .Select(tag => new
                {
                    PostId = postId,
                    TagId = tag.Id
                })
                .ToList();

            if (relationsToInsert.Count != 0)
            {
                await connection.ExecuteAsync(
                    """
                                    INSERT INTO PostsTagsRelations (PostId, TagId)
                                      VALUES (@PostId, @TagId)
                    """,
                    relationsToInsert,
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
    public async Task AddTagsToExistingPost(int postId, List<PostsTagsRelations> existingPostsTagsRelations, IEnumerable<Tag> tags)
    {
        await AddTagsToPost(postId, existingPostsTagsRelations, tags);
    }
}