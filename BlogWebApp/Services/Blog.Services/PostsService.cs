// <copyright file="PostsService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Blog.Data.Specifications;

namespace Blog.Services
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Blog.Contracts.V1.Responses.Chart;
    using Blog.Core.Helpers;
    using Blog.Data.Models;
    using Blog.Data.Repository;
    using Blog.Services.Core;
    using Blog.Services.Core.Dtos;
    using Blog.Services.Core.Dtos.Posts;
    using Blog.Services.Core.Dtos.User;
    using Blog.Services.GeneralService;
    using Blog.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Posts service.
    /// </summary>
    public class PostsService : GeneralService<Post>, IPostsService
    {
        /// <summary>
        /// The comments service.
        /// </summary>
        private readonly ICommentsService commentsService;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// The posts tags relations service.
        /// </summary>
        private readonly IPostsTagsRelationsService postsTagsRelationsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostsService"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        /// <param name="commentsService">The comments service.</param>
        /// <param name="postsTagsRelationsService">The tags service.</param>
        /// <param name="mapper">The mapper.</param>
        public PostsService(
            IRepository<Post> repo,
            ICommentsService commentsService,
            IMapper mapper,
            IPostsTagsRelationsService postsTagsRelationsService)
            : base(repo)
        {
            this.commentsService = commentsService;
            this.postsTagsRelationsService = postsTagsRelationsService;
            this.mapper = mapper;
        }

        /// <inheritdoc cref="IPostsService"/>
        public async Task<Post> GetPostAsync(int id)
        {
            return await this.Repository.Table

                // .Include(c => c.Comments)
                .Where(new PostSpecification(x => x.Id.Equals(id)).Filter)
                .Include(x => x.PostsTagsRelations)
                    .ThenInclude(x => x.Tag)

                // .OrderByDescending(d => d.) comments order by date descending
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc cref="IPostsService"/>
        public async Task<PostShowViewDto> GetPost(int postId, SortParametersDto sortParameters)
        {
            var post = await this.Repository.Table
                .Where(new PostSpecification(x => x.Id.Equals(postId)).Filter)
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
                Comments = await this.commentsService.GetPagedCommentsByPostId(postId, sortParameters),
            };

            post.PostsTagsRelations = null;
            postModel.Post = this.mapper.Map<Post, PostViewDto>(post);
            postModel.Post.Author = this.mapper.Map<ApplicationUser, ApplicationUserDto>(post.Author);
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
                .Where(new PostSpecification(x => x.Id.Equals(id)).Filter)
                .Include(x => x.Author)
                .ThenInclude(x => x.Profile)

                // .OrderByDescending(d => d.) comments order by date descending
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc cref="IPostsService"/>
        public async Task<PostsViewDto> GetPostsAsync(PostsSearchParametersDto searchParameters)
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

            if (!string.IsNullOrEmpty(searchParameters.Tag))
            {
                postsList = postsList.Where(post =>
                    post.PostsTagsRelations.Any(x => x.Tag.Title.ToLower().Equals(searchParameters.Tag.ToLower()))).ToList();
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
                    Author = this.mapper.Map<ApplicationUser, ApplicationUserDto>(post.Author),
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

        /// <inheritdoc cref="IPostsService"/>
        public async Task<PostsViewDto> GetUserPostsAsync(string userId, PostsSearchParametersDto searchParameters)
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

            if (!string.IsNullOrEmpty(searchParameters.Tag))
            {
                postsList = postsList.Where(post =>
                    post.PostsTagsRelations.Any(x => x.Tag.Title.ToLower().Equals(searchParameters.Tag.ToLower()))).ToList();
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
                    Author = this.mapper.Map<ApplicationUser, ApplicationUserDto>(post.Author),
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

        /// <inheritdoc cref="IPostsService"/>
        public async Task InsertAsync(Post post, IEnumerable<Tag> tags)
        {
            await this.InsertAsync(post);
            post.PostsTagsRelations = new Collection<PostsTagsRelations>();
            await this.postsTagsRelationsService.AddTagsToPost(post.Id, post.PostsTagsRelations.ToList(), tags).ConfigureAwait(false);
        }

        /// <inheritdoc cref="IPostsService"/>
        public async Task<ChartDataModel> GetPostsActivity()
            => new()
            {
                Name = "Posts",
                Series = await Repository.TableNoTracking
                    .GroupBy(x => x.CreatedAt)
                    .Select(x => new ChartItem
                    {
                        Name = x.Key.ToString("dd/MM/yyyy"),
                        Value = x.Count(),
                    })
                    .ToListAsync(),
            };
    }
}