// <copyright file="PostsTagsRelationsService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Data.Repository;
using GeneralService;
using Interfaces;

/// <summary>
/// Posts tags relations service.
/// </summary>
/// <seealso cref="GeneralService{PostsTagsRelations}" />
/// <seealso cref="IPostsTagsRelationsService" />
public class PostsTagsRelationsService : GeneralService<PostsTagsRelations>, IPostsTagsRelationsService
{
    /// <summary>
    /// The tags service.
    /// </summary>
    private readonly ITagsService tagsService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostsTagsRelationsService"/> class.
    /// </summary>
    /// <param name="repo">The repo.</param>
    /// <param name="tagsService">The tags service.</param>
    public PostsTagsRelationsService(
        IRepository<PostsTagsRelations> repo,
        ITagsService tagsService)
        : base(repo)
    {
        this.tagsService = tagsService;
    }

    /// <inheritdoc cref="IPostsTagsRelationsService"/>
    public async Task AddTagsToPost(int postId, List<PostsTagsRelations> postsTagsRelations, IEnumerable<Tag> tags)
    {
        try
        {
            postsTagsRelations = postsTagsRelations ?? new List<PostsTagsRelations>();
            var existingTags = await this.tagsService.GetAllAsync().ConfigureAwait(false);

            foreach (var tag in tags)
            {
                var tagFromDatabase = existingTags.FirstOrDefault(x => x.Title.ToLower().Equals(tag.Title.ToLower()));
                var tagExistsInPost =
                    postsTagsRelations.FirstOrDefault(x =>
                        x.Tag.Title.ToLower().Equals(tag.Title.ToLower()));
                PostsTagsRelations postsTagsRelation;
                if (tagExistsInPost != null)
                {
                    continue;
                }

                if (tagFromDatabase != null)
                {
                    postsTagsRelation = new PostsTagsRelations
                    {
                        PostId = postId,
                        TagId = tagFromDatabase.Id,
                        Tag = tagFromDatabase,
                    };
                }
                else
                {
                    var tagToCreate = new Tag
                    {
                        Title = tag.Title,
                    };
                    await this.tagsService.InsertAsync(tagToCreate);
                    postsTagsRelation = new PostsTagsRelations
                    {
                        PostId = postId,
                        TagId = tagToCreate.Id,
                        Tag = tagToCreate,
                    };
                }

                postsTagsRelations.Add(postsTagsRelation);
            }

            await this.InsertAsync(postsTagsRelations).ConfigureAwait(false);
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task AddTagsToExistingPost(int postId, List<PostsTagsRelations> existingPostsTagsRelations, IEnumerable<Tag> tags)
    {
        try
        {
            existingPostsTagsRelations = existingPostsTagsRelations ?? new List<PostsTagsRelations>();
            var postsTagsRelations = new List<PostsTagsRelations>();
            var existingTags = await this.tagsService.GetAllAsync().ConfigureAwait(false);

            foreach (var tag in tags)
            {
                var tagFromDatabase = existingTags.FirstOrDefault(x => x.Title.ToLower().Equals(tag.Title.ToLower()));
                var tagExistsInPost =
                    existingPostsTagsRelations.FirstOrDefault(x =>
                        x.Tag.Title.ToLower().Equals(tag.Title.ToLower()));
                PostsTagsRelations postsTagsRelation;
                if (tagExistsInPost != null)
                {
                    continue;
                }

                if (tagFromDatabase != null)
                {
                    postsTagsRelation = new PostsTagsRelations
                    {
                        PostId = postId,
                        TagId = tagFromDatabase.Id,
                        Tag = tagFromDatabase,
                    };
                }
                else
                {
                    var tagToCreate = new Tag
                    {
                        Title = tag.Title,
                    };
                    await this.tagsService.InsertAsync(tagToCreate);
                    postsTagsRelation = new PostsTagsRelations
                    {
                        PostId = postId,
                        TagId = tagToCreate.Id,
                        Tag = tagToCreate,
                    };
                }

                postsTagsRelations.Add(postsTagsRelation);
            }

            await this.InsertAsync(postsTagsRelations).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}