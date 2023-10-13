namespace Blog.Web.SwaggerExamples.Responses.PostsResponses;

using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Responses.CommentsResponses;
using Blog.Contracts.V1.Responses.PostsResponses;
using Blog.Contracts.V1.Responses.TagsResponses;
using Core.Consts;

/// <summary>
/// Post response example.
/// </summary>
/// <seealso cref="IExamplesProvider{PostResponse}" />
public class PostResponseExample : IExamplesProvider<PostResponse>
{
    /// <inheritdoc cref="IExamplesProvider{T}"/>
    public PostResponse GetExamples()
    {
        return new PostResponse
        {
            Title = SwaggerExamplesConsts.PostViewResponseExample.Title,
            Description = SwaggerExamplesConsts.PostViewResponseExample.Description,
            Content = SwaggerExamplesConsts.PostViewResponseExample.Content,
            ImageUrl = SwaggerExamplesConsts.PostViewResponseExample.ImageUrl,
            AuthorId = Guid.NewGuid().ToString(),

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

            PostsTagsRelations = new List<PostTagRelationsResponse>
            {
                new ()
                {
                    TagId = 0,
                    Tag = new TagResponse
                    {
                        Title = SwaggerExamplesConsts.TagResponseExample.Title + "1"
                    }
                },

                new ()
                {
                    TagId = 0,
                    Tag = new TagResponse
                    {
                        Title = SwaggerExamplesConsts.TagResponseExample.Title + "2"
                    }
                }
            },
        };
    }
}