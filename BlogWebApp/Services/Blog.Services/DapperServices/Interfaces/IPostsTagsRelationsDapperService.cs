// <copyright file="IPostsTagsRelationsDapperService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Blog.Data.Models;
using Blog.EntityServices.GeneralService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.EntityServices.DapperServices.Interfaces;

/// <summary>
/// Post tag relations dapper service interface.
/// </summary>
/// <seealso cref="IGeneralDapperService{PostsTagsRelations}" />
public interface IPostsTagsRelationsDapperService : IGeneralDapperService<PostsTagsRelations>
{
    /// <summary>
    /// Adds the tags to post.
    /// </summary>
    /// <param name="postId">The post identifier.</param>
    /// <param name="postsTagsRelations">The posts tags relations.</param>
    /// <param name="tags">The tags.</param>
    /// <returns>Task.</returns>
    Task AddTagsToPost(int postId, List<PostsTagsRelations> postsTagsRelations, IEnumerable<Tag> tags);

    /// <summary>
    /// Add tags to existing post.
    /// </summary>
    /// <param name="postId">The post id.</param>
    /// <param name="existingPostsTagsRelations">The existing posts tags relations.</param>
    /// <param name="tags">THe tags.</param>
    /// <returns>Task.</returns>
    Task AddTagsToExistingPost(int postId, List<PostsTagsRelations> existingPostsTagsRelations, IEnumerable<Tag> tags);
}