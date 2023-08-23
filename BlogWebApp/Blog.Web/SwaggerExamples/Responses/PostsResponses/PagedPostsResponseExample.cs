namespace Blog.Web.SwaggerExamples.Responses.PostsResponses;

using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Responses;
using Blog.Contracts.V1.Responses.CommentsResponses;
using Blog.Contracts.V1.Responses.PostsResponses;
using Blog.Contracts.V1.Responses.TagsResponses;
using Core.Consts;

/// <summary>
/// Paged posts response example.
/// </summary>
/// <seealso cref="IExamplesProvider{PagedPostsResponse}" />
public class PagedPostsResponseExample : IExamplesProvider<PagedPostsResponse>
{
    /// <inheritdoc cref="IExamplesProvider{T}"/>
    public PagedPostsResponse GetExamples()
    {
        return new PagedPostsResponse
        {
            Posts = new List<PostViewResponse> 
            {
                new PostViewResponse
                {
                    Id = 0,
                    Title = SwaggerExamplesConsts.PostViewResponseExample.Title + "1",
                    Description = SwaggerExamplesConsts.PostViewResponseExample.Description,
                    Content = SwaggerExamplesConsts.PostViewResponseExample.Content,
                    ImageUrl = SwaggerExamplesConsts.PostViewResponseExample.ImageUrl,
                    AuthorId = Guid.NewGuid().ToString(),
                    CommentsCount = 10,

                    Comments = new List<CommentResponse>
                    {
                        new CommentResponse
                        {
                            Id = 0,
                            PostId = 0,
                            CommentBody = SwaggerExamplesConsts.CommentResponseExample.CommentBody + "1",
                            CreatedAt = DateTime.Now,
                            UserId = Guid.NewGuid().ToString(),
                        },

                        new CommentResponse
                        {
                            Id = 0,
                            PostId = 0,
                            CommentBody = SwaggerExamplesConsts.CommentResponseExample.CommentBody + "2",
                            CreatedAt = DateTime.Now,
                            UserId = Guid.NewGuid().ToString(),
                        },
                    },

                    Tags = new List<TagResponse>
                    {
                        new TagResponse
                        {
                            Id = 0,
                            Title = SwaggerExamplesConsts.TagRequestExample.Title + "1",
                        },

                        new TagResponse
                        {
                            Id = 0,
                            Title = SwaggerExamplesConsts.TagRequestExample.Title + "2",
                        },
                    },
                },

                new PostViewResponse
                {
                    Id = 0,
                    Title = SwaggerExamplesConsts.PostViewResponseExample.Title + "2",
                    Description = SwaggerExamplesConsts.PostViewResponseExample.Description,
                    Content = SwaggerExamplesConsts.PostViewResponseExample.Content,
                    ImageUrl = SwaggerExamplesConsts.PostViewResponseExample.ImageUrl,
                    AuthorId = Guid.NewGuid().ToString(),
                    CommentsCount = 10,

                    Comments = new List<CommentResponse>
                    {
                        new CommentResponse
                        {
                            Id = 0,
                            PostId = 0,
                            CommentBody = SwaggerExamplesConsts.CommentResponseExample.CommentBody + "1",
                            CreatedAt = DateTime.Now,
                            UserId = Guid.NewGuid().ToString(),
                        },

                        new CommentResponse
                        {
                            Id = 0,
                            PostId = 0,
                            CommentBody = SwaggerExamplesConsts.CommentResponseExample.CommentBody + "2",
                            CreatedAt = DateTime.Now,
                            UserId = Guid.NewGuid().ToString(),
                        },
                    },

                    Tags = new List<TagResponse>
                    {
                        new TagResponse
                        {
                            Id = 0,
                            Title = SwaggerExamplesConsts.TagResponseExample.Title + "1",
                        },

                        new TagResponse
                        {
                            Id = 0,
                            Title = SwaggerExamplesConsts.TagResponseExample.Title + "2",
                        },
                    },
                },

                new PostViewResponse
                {
                    Id = 0,
                    Title = SwaggerExamplesConsts.PostViewResponseExample.Title + "3",
                    Description = SwaggerExamplesConsts.PostViewResponseExample.Description,
                    Content = SwaggerExamplesConsts.PostViewResponseExample.Content,
                    ImageUrl = SwaggerExamplesConsts.PostViewResponseExample.ImageUrl,
                    AuthorId = Guid.NewGuid().ToString(),
                    CommentsCount = 10,

                    Comments = new List<CommentResponse>
                    {
                        new CommentResponse
                        {
                            Id = 0,
                            PostId = 0,
                            CommentBody = SwaggerExamplesConsts.CommentResponseExample.CommentBody + "1",
                            CreatedAt = DateTime.Now,
                            UserId = Guid.NewGuid().ToString(),
                        },

                        new CommentResponse
                        {
                            Id = 0,
                            PostId = 0,
                            CommentBody = SwaggerExamplesConsts.CommentResponseExample.CommentBody + "2",
                            CreatedAt = DateTime.Now,
                            UserId = Guid.NewGuid().ToString(),
                        },
                    },

                    Tags = new List<TagResponse>
                    {
                        new TagResponse
                        {
                            Id = 0,
                            Title = SwaggerExamplesConsts.TagResponseExample.Title + "1",
                        },

                        new TagResponse
                        {
                            Id = 0,
                            Title = SwaggerExamplesConsts.TagResponseExample.Title + "2",
                        },
                    },
                },
            },

            PageInfo = new PageInfoResponse
            {
                PageNumber = 1,
                PageSize = 10,
                TotalItems = 100,
            },
        };
    }
}