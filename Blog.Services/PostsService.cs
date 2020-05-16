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
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostsService"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        /// <param name="commentsService">The comments service.</param>
        /// <param name="mapper">The mapper.</param>
        public PostsService(
            IRepository<Post> repo,
            ICommentsService commentsService,
            IMapper mapper)
            : base(repo)
        {
            this._commentsService = commentsService;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<Post> GetPostAsync(int id)
        {
            return await this.Repository.Table

                // .Include(c => c.Comments)
                .Where(x => x.Id.Equals(id))
                .Include(x => x.Author)

                // .OrderByDescending(d => d.) comments order by date descending
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<PostShowViewDto> GetPost(int postId, SortParametersDto sortParameters)
        {
            var postModel = new PostShowViewDto
            {
                Post = await this.Repository.Table
                    .Where(x => x.Id.Equals(postId))
                    .Include(x => x.Author)
                    .ThenInclude(x => x.Profile)
                    .FirstOrDefaultAsync(),
                Comments = await this._commentsService.GetPagedCommentsByPostId(postId, sortParameters),
            };
            /*
            postModel.Profile = db.Profiles.Where(pr => pr.ApplicationUser.Equals(postModel.Post.Author)).FirstOrDefault();
            */
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
                    Tags = post.Tags,
                    AuthorId = post.AuthorId,
                    Author = this._mapper.Map<ApplicationUser, ApplicationUserDto>(post.Author),
                    CommentsCount = post.Comments.Count,
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
                .Include(x => x.Comments).ToListAsync();

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
                    Tags = post.Tags,
                    AuthorId = post.AuthorId,
                    Author = _mapper.Map<ApplicationUser, ApplicationUserDto>(post.Author),
                    CommentsCount = post.Comments.Count,
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
    }
}