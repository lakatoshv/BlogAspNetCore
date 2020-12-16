// <copyright file="PostsTagsRelationsService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Blog.Data.Models;
using Blog.Data.Repository;
using Blog.Services.GeneralService;
using Blog.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Services
{
    /// <summary>
    /// Posts tags relations service.
    /// </summary>
    /// <seealso cref="GeneralService{PostsTagsRelations}" />
    /// <seealso cref="IPostsTagsRelationsService" />
    public class PostsTagsRelationsService : GeneralService<PostsTagsRelations>, IPostsTagsRelationsService
    {
        private readonly ITagsService _tagsService;

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
            _tagsService = tagsService;
        }

        /// <inheritdoc />
        public async Task AddTagsToPost(int postId, List<PostsTagsRelations> postsTagsRelations, IEnumerable<Tag> tags)
        {
            postsTagsRelations = postsTagsRelations ?? new List<PostsTagsRelations>();
            var existingTags = await this._tagsService.GetAllAsync().ConfigureAwait(false);

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
                    await this._tagsService.InsertAsync(tagToCreate);
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
    }
}