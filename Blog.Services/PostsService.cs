﻿// <copyright file="PostsServices.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Blog.Core.Helpers;
    using Data.Models;
    using Data.Repository;
    using Core;
    using Core.Dtos;
    using Core.Dtos.Posts;
    using GeneralService;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Posts service.
    /// </summary>
    public class PostsService : GeneralService<Post>, IPostsService
    {
        // private ICommentService _commentsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostsService"/> class.
        /// </summary>
        /// <param name="repo">repo.</param>
        public PostsService(
            IRepository<Post> repo)

            // ,ICommentService commentsService
            : base(repo)
        {
            // _commentsService = commentsService;
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
        public async Task<PostShowViewDto> GetPostWithComments(int postId, SortParametersDto sortParameters)
        {
            PostShowViewDto postModel = new PostShowViewDto
            {
                Post = await this.Repository.Table
                    .Where(x => x.Id.Equals(postId))
                    .Include(x => x.Author)
                    .FirstOrDefaultAsync(),
            };
            /*
            postModel.Profile = db.Profiles.Where(pr => pr.ApplicationUser.Equals(postModel.Post.Author)).FirstOrDefault();
            */
            if (postModel.Post == null)
            {
                return null;
            }

            /*
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var author = postModel.Post.Author;
            ApplicationUser user = userManager.FindByIdAsync(author).Result;
            if (user != null)
               postModel.Post.Author = user.UserName;

            var tags = db.Tags.Where(tag => tag.PostId.Equals(postId)).ToList();
            foreach (var tag in tags)
            {
               postModel.Post.PostTags.Add(tag);
            }
            */

            // postModel.Comments = await _commentsService.GetPagedCommentsByPostId(postId, sortParameters);
            return postModel;
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

                // .OrderByDescending(d => d.) comments order by date descending
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<PostsViewDto> GetPostsAsync(SearchParametersDto searchParameters)
        {
            PostsViewDto posts = new PostsViewDto();
            var postsList = await this.Repository.TableNoTracking.Include(x => x.Author)./*Include(table => table.Comments).*/ToListAsync();
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
                PostViewDto p = new PostViewDto()
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
                    Author = post.Author,

                    // CommentsCount = post.Comments.Count
                };
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
            PostsViewDto posts = new PostsViewDto();
            var postsList = await this.Repository.TableNoTracking.Include(x => x.Author)./*Include(table => table.Comments).*/Where(post => post.AuthorId.Equals(userId)).ToListAsync();

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
                PostViewDto p = new PostViewDto()
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
                    Author = post.Author,

                    // CommentsCount = post.Comments.Count
                };
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