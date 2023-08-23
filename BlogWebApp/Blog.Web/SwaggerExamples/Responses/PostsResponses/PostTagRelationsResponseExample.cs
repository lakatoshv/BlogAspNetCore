namespace Blog.Web.SwaggerExamples.Responses.PostsResponses;

using System;
using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Responses.PostsResponses;
using Blog.Contracts.V1.Responses.TagsResponses;
using Core.Consts;

/// <summary>
/// Post tag relations response example.
/// </summary>
/// <seealso cref="IExamplesProvider{PostTagRelationsResponse}" />
public class PostTagRelationsResponseExample : IExamplesProvider<PostTagRelationsResponse>
{
    /// <inheritdoc cref="IExamplesProvider{T}"/>
    public PostTagRelationsResponse GetExamples()
    {
        return new PostTagRelationsResponse
        {
            PostId = 0,
            Post = new PostResponse
            {
                Title = SwaggerExamplesConsts.PostViewResponseExample.Title,
                Description = SwaggerExamplesConsts.PostViewResponseExample.Description,
                Content = SwaggerExamplesConsts.PostViewResponseExample.Content,
                ImageUrl = SwaggerExamplesConsts.PostViewResponseExample.ImageUrl,
                AuthorId = Guid.NewGuid().ToString(),
            },

            TagId = 0,
            Tag = new TagResponse
            {
                Title = SwaggerExamplesConsts.TagResponseExample.Title + "1",
            }
        };
    }
}