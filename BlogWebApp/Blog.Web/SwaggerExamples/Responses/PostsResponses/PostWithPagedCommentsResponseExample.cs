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
/// Post with paged comments response example.
/// </summary>
/// <seealso cref="IExamplesProvider{PostWithPagedCommentsResponse}" />
public class PostWithPagedCommentsResponseExample : IExamplesProvider<PostWithPagedCommentsResponse>
{
    /// <inheritdoc cref="IExamplesProvider{T}"/>
    public PostWithPagedCommentsResponse GetExamples()
    {
        return new PostWithPagedCommentsResponse
        {
            Post = new PostViewResponse
            {
                Id = 0,
                Title = SwaggerExamplesConsts.PostViewResponseExample.Title,
                Description = SwaggerExamplesConsts.PostViewResponseExample.Description,
                Content = SwaggerExamplesConsts.PostViewResponseExample.Content,
                ImageUrl = SwaggerExamplesConsts.PostViewResponseExample.ImageUrl,
                AuthorId = Guid.NewGuid().ToString(),
                CommentsCount = 10
            },
            Comments = new PagedCommentsResponse
            {
                Comments = new List<CommentResponse> 
                {
                    new ()
                    {
                        Id = 0,
                        PostId = 0,
                        CommentBody = SwaggerExamplesConsts.CommentResponseExample.CommentBody + "1",
                        CreatedAt = DateTime.Now,
                        UserId = Guid.NewGuid().ToString()
                    },

                    new ()
                    {
                        Id = 0,
                        PostId = 0,
                        CommentBody = SwaggerExamplesConsts.CommentResponseExample.CommentBody + "2",
                        CreatedAt = DateTime.Now,
                        UserId = Guid.NewGuid().ToString()
                    },
                },

                PageInfo = new PageInfoResponse
                {
                    PageNumber = 1,
                    PageSize = 10,
                    TotalItems = 100
                },
            },

            Tags = new List<TagResponse>
            {
                new ()
                {
                    Id = 0,
                    Title = SwaggerExamplesConsts.TagResponseExample.Title + "1"
                },

                new ()
                {
                    Id = 0,
                    Title = SwaggerExamplesConsts.TagResponseExample.Title + "2"
                }
            },
        };
    }
}