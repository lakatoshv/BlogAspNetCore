namespace Blog.Web.SwaggerExamples.Responses.PostsResponses;

using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Responses.CommentsResponses;
using Blog.Contracts.V1.Responses.PostsResponses;
using Blog.Contracts.V1.Responses.TagsResponses;
using Core.Consts;

/// <summary>
/// Post view response example.
/// </summary>
/// <seealso cref="IExamplesProvider{PostViewResponse}" />
public class PostViewResponseExample : IExamplesProvider<PostViewResponse>
{
    /// <inheritdoc cref="IExamplesProvider{T}"/>
    public PostViewResponse GetExamples()
    {
        return new PostViewResponse
        {
            Id = 0,
            Title = SwaggerExamplesConsts.PostViewResponseExample.Title,
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
        };
    }
}