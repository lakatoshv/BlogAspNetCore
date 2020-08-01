// <copyright file="PostsServices.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Blog.Core.Helpers;
    using Core.Dtos.User;
    using Core;
    using Core.Dtos;
    using Core.Dtos.Posts;
    using Data.Models;
    using Data.Repository;
    using GeneralService;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Posts service.
    /// </summary>
    public class PostsService : GeneralService<Post>, IPostsService
    {
        /// <summary>
        /// The comments service.
        /// </summary>
        private ICommentsService _commentsService;

        /// <summary>
        /// The tags service.
        /// </summary>
        private readonly ITagsService _tagsService;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostsService"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        /// <param name="commentsService">The comments service.</param>
        /// <param name="tagsService">The tags service.</param>
        /// <param name="mapper">The mapper.</param>
        public PostsService(
            IRepository<Post> repo,
            ICommentsService commentsService,
            ITagsService tagsService,
            IMapper mapper)
            : base(repo)
        {
            this._commentsService = commentsService;
            this._tagsService = tagsService;
            this._mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<Post> GetPostAsync(int id)
        {
            return await this.Repository.Table

                // .Include(c => c.Comments)
                .Where(x => x.Id.Equals(id))
                .Include(x => x.Author)
                .Include(x => x.PostsTagsRelations)
                .ThenInclude(x => x.Tag)

                // .OrderByDescending(d => d.) comments order by date descending
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<PostShowViewDto> GetPost(int postId, SortParametersDto sortParameters)
        {
            var post = await this.Repository.Table
                .Where(x => x.Id.Equals(postId))
                .Include(x => x.Author)
                .ThenInclude(x => x.Profile)
                .Include(x => x.PostsTagsRelations)
                .ThenInclude(x => x.Tag)
                .FirstOrDefaultAsync();

            var postModel = new PostShowViewDto
            {
                Tags = post.PostsTagsRelations.Select(x => new TagViewDto
                {
                    Id = x.Tag.Id,
                    Title = x.Tag.Title,
                }).ToList(),
                Comments = await this._commentsService.GetPagedCommentsByPostId(postId, sortParameters),
            };

            post.PostsTagsRelations = null;
            postModel.Post = this._mapper.Map<Post, PostViewDto>(post);
            postModel.Post.Author = this._mapper.Map<ApplicationUser, ApplicationUserDto>(post.Author);
            postModel.Post.Author.Profile.User = null;

            return postModel.Post == null ? null : postModel;
        }

        /// <summary>
        /// Async get post without comments.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>Task.</returns>
        public async Task<Post> GetPostWithoutCommentsAsync(int id)
        {
            return await this.Repository.Table

                // .Include(c => c.Comments)
                .Where(x => x.Id.Equals(id))
                .Include(x => x.Author)
                .ThenInclude(x => x.Profile)

                // .OrderByDescending(d => d.) comments order by date descending
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<PostsViewDto> GetPostsAsync(SearchParametersDto searchParameters)
        {
            var posts = new PostsViewDto();
            var postsList = await this.Repository.TableNoTracking
                .Include(x => x.Author)
                    .ThenInclude(x => x.Profile)
                .Include(table => table.Comments)
                .Include(x => x.PostsTagsRelations)
                    .ThenInclude(x => x.Tag)
                .ToListAsync();

            if (!string.IsNullOrEmpty(searchParameters.Search))
            {
                postsList = postsList.Where(post => post.Title.ToLower().Contains(searchParameters.Search.ToLower())).ToList();
            }

            var postsCount = postsList.Count;

            if (searchParameters.SortParameters.CurrentPage != null && searchParameters.SortParameters.PageSize != null)
            {
                postsList = postsList.AsQueryable().OrderBy(searchParameters.SortParameters)
                    .Skip((searchParameters.SortParameters.CurrentPage.Value - 1) *
                        searchParameters.SortParameters.PageSize.Value)
                    .Take(searchParameters.SortParameters.PageSize.Value).ToList();
            }

            posts.Posts = new List<PostViewDto>();
            postsList.ForEach(post =>
            {
                var p = new PostViewDto()
                {
                    Id = post.Id,
                    Title = post.Title,
                    Description = post.Description,
                    Content = post.Content,
                    Seen = post.Seen,
                    Likes = post.Likes,
                    Dislikes = post.Dislikes,
                    ImageUrl = post.ImageUrl,
                    AuthorId = post.AuthorId,
                    Author = this._mapper.Map<ApplicationUser, ApplicationUserDto>(post.Author),
                    CommentsCount = post.Comments.Count,
                    Tags = post.PostsTagsRelations.Select(x => new TagViewDto
                    {
                        Id = x.Tag.Id,
                        Title = x.Tag.Title,
                    }).ToList(),
                };
                post.Author.Profile.User = null;
                posts.Posts.Add(p);
            });

            if (searchParameters.SortParameters.CurrentPage != null && searchParameters.SortParameters.PageSize != null)
            {
                posts.PageInfo = new PageInfo
                {
                    PageNumber = searchParameters.SortParameters.CurrentPage.Value,
                    PageSize = searchParameters.SortParameters.PageSize.Value,
                    TotalItems = postsCount,
                };
            }

            return posts;
        }

        /// <inheritdoc/>
        public async Task<PostsViewDto> GetUserPostsAsync(string userId, SearchParametersDto searchParameters)
        {
            var posts = new PostsViewDto();
            var postsList = await this.Repository.TableNoTracking
                .Where(post => post.AuthorId.Equals(userId))
                .Include(x => x.Author)
                    .ThenInclude(x => x.Profile)
                .Include(x => x.Comments)
                .Include(x => x.PostsTagsRelations)
                    .ThenInclude(x => x.Tag)
                .ToListAsync();

            if (!string.IsNullOrEmpty(searchParameters.Search))
            {
                postsList = postsList.Where(post => post.Title.ToLower().Contains(searchParameters.Search.ToLower())).ToList();
            }

            postsList = postsList.AsQueryable().OrderBy(searchParameters.SortParameters).ToList();

            var postsCount = postsList.Count;
            if (searchParameters.SortParameters.CurrentPage != null && searchParameters.SortParameters.PageSize != null)
            {
                postsList = postsList.AsQueryable().OrderBy(searchParameters.SortParameters)
                    .Skip((searchParameters.SortParameters.CurrentPage.Value - 1) *
                          searchParameters.SortParameters.PageSize.Value)
                    .Take(searchParameters.SortParameters.PageSize.Value).ToList();
            }

            posts.Posts = new List<PostViewDto>();
            postsList.ForEach(post =>
            {
                var p = new PostViewDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Description = post.Description,
                    Content = post.Content,
                    Seen = post.Seen,
                    Likes = post.Likes,
                    Dislikes = post.Dislikes,
                    ImageUrl = post.ImageUrl,
                    AuthorId = post.AuthorId,
                    Author = this._mapper.Map<ApplicationUser, ApplicationUserDto>(post.Author),
                    CommentsCount = post.Comments.Count,
                    Tags = post.PostsTagsRelations.Select(x => new TagViewDto
                    {
                        Id = x.Tag.Id,
                        Title = x.Tag.Title,
                    }).ToList(),
                };
                post.Author.Profile.User = null;
                posts.Posts.Add(p);
            });

            if (searchParameters.SortParameters.CurrentPage != null && searchParameters.SortParameters.PageSize != null)
            {
                posts.PageInfo = new PageInfo
                {
                    PageNumber = searchParameters.SortParameters.CurrentPage.Value,
                    PageSize = searchParameters.SortParameters.PageSize.Value,
                    TotalItems = postsCount,
                };
            }

            return posts;
        }

        /// <inheritdoc/>
        public async Task InsertAsync(Post post, IEnumerable<string> tags)
        {
            await this.InsertAsync(post);
            await this.AddTagsToPost(post, tags).ConfigureAwait(false);
        }

        /// <summary>
        /// Add tags to post.
        /// </summary>
        /// <param name="post">The post.</param>
        /// <param name="tags">The tags.</param>
        /// <returns>Task.</returns>
        private async Task AddTagsToPost(Post post, IEnumerable<string> tags)
        {
            post.PostsTagsRelations = post.PostsTagsRelations ?? new List<PostsTagsRelations>();
            var existingTags = await this._tagsService.GetAllAsync().ConfigureAwait(false);

            foreach (var tagToCreate in tags)
            {
                var tag = existingTags.FirstOrDefault(x => x.Title.ToLower().Equals(tagToCreate.ToLower()));
                PostsTagsRelations postsTagsRelations;
                if (tag != null)
                {
                    postsTagsRelations = new PostsTagsRelations
                    {
                        PostId = post.Id,
                        TagId = tag.Id,
                    };
                }
                else
                {
                    postsTagsRelations = new PostsTagsRelations
                    {
                        PostId = post.Id,
                        Tag = new Tag
                        {
                            Title = tagToCreate,
                        },
                    };
                }

                post.PostsTagsRelations.Add(postsTagsRelations);
            }

            await this.UpdateAsync(post);
        }
    }
}