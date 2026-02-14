// <copyright file="PostsService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Blog.Services.Core.Dtos.Exports;

namespace Blog.EntityServices.EntityFrameworkServices;

using AutoMapper;
using Interfaces;
using Blog.Services.Core;
using Blog.Services.Core.Dtos;
using Blog.Services.Core.Dtos.Posts;
using Blog.Services.Core.Dtos.User;
using Contracts.V1.Responses.Chart;
using Core.Helpers;
using Data.Models;
using Data.Repository;
using Data.Specifications;
using GeneralService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Posts service.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="PostsService"/> class.
/// </remarks>
/// <param name="repo">The repo.</param>
/// <param name="commentsService">The comments service.</param>
/// <param name="postsTagsRelationsService">The tags service.</param>
/// <param name="mapper">The mapper.</param>
/// <param name="exportsService">The exports service.</param>
public class PostsService(
    IRepository<Post> repo,
    ICommentsService commentsService,
    IMapper mapper,
    IPostsTagsRelationsService postsTagsRelationsService)
    //IExportsService exportsService)
    : GeneralService<Post>(repo), IPostsService
{
    /// <summary>
    /// The comments service.
    /// </summary>
    private readonly ICommentsService commentsService = commentsService;

    /// <summary>
    /// The mapper.
    /// </summary>
    private readonly IMapper mapper = mapper;

    /// <summary>
    /// The posts tags relations service.
    /// </summary>
    private readonly IPostsTagsRelationsService postsTagsRelationsService = postsTagsRelationsService;

    /// <summary>
    /// The exports service.
    /// </summary>
   // private readonly IExportsService exportsService = exportsService;

    /// <inheritdoc cref="IPostsService"/>
    public async Task<Post> GetPostAsync(int id)
        => await this.Repository.Table

            // .Include(c => c.Comments)
            .Where(new PostSpecification(x => x.Id.Equals(id)).Filter)
            .Include(x => x.PostsTagsRelations)
            .ThenInclude(x => x.Tag)

            // .OrderByDescending(d => d.) comments order by date descending
            .FirstOrDefaultAsync();

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
        => await this.Repository.Table

            // .Include(c => c.Comments)
            .Where(new PostSpecification(x => x.Id.Equals(id)).Filter)
            .Include(x => x.Author)
            .ThenInclude(x => x.Profile)

            // .OrderByDescending(d => d.) comments order by date descending
            .FirstOrDefaultAsync();

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

    /// <inheritdoc cref="IPostsService"/>
    public async Task<byte[]> ExportPostsToExcel(PostsSearchParametersDto searchParameters)
    {
        try
        {
            var posts = await GetPostsAsync(searchParameters);

            var exportRequest = new ExportDataIntoExcelDto
            {
                Headers =
                [
                    new ("Title"),
                    new ("Description"),
                    new ("Content"),
                    new ("Author"),
                    new ("Seen"),
                    new ("Likes"),
                    new ("Dislikes"),
                    new ("ImageUrl"),
                    new ("Tags"),
                    new ("Comments count")
                ],
                Rows = []
            };

            foreach (var post in posts.Posts)
            {
                var tags = string.Empty;
                _ = post.Tags.Select(x => tags = string.IsNullOrEmpty(tags) ? x.Title : $"{tags}, {x.Title}");

                var dataTable = new DataTable();
                var row = dataTable.NewRow();
                row[0] = post.Title;
                row[1] = post.Description;
                row[2] = post.Content;
                row[3] = $"{post.Author.FirstName} {post.Author.LastName}({post.Author.Email})";
                row[4] = post.Seen;
                row[5] = post.Likes;
                row[6] = post.Dislikes;
                row[7] = post.ImageUrl;
                row[8] = tags;
                row[9] = post.Comments.Count;

                exportRequest.Rows.Add(row);
            }

            return null; //this.exportsService.ExportDataIntoExcel(exportRequest);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}