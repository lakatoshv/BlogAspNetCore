namespace Blog.Web.SwaggerExamples.Requests.PostsRequests;

using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using Blog.Contracts.V1.Requests.PostsRequests;
using Blog.Contracts.V1.Requests.TagsRequests;
using Core.Consts;

/// <summary>
/// Create post request example.
/// </summary>
/// <seealso cref="IExamplesProvider{CreatePostRequest}" />
public class CreatePostRequestExample : IExamplesProvider<CreatePostRequest>
{
    /// <inheritdoc cref="IExamplesProvider{T}"/>
    public CreatePostRequest GetExamples()
    {
        return new CreatePostRequest
        {
            Title = SwaggerExamplesConsts.CreatePostRequestExample.Title,
            Description = SwaggerExamplesConsts.CreatePostRequestExample.Description,
            Content = SwaggerExamplesConsts.CreatePostRequestExample.Content,
            ImageUrl = SwaggerExamplesConsts.CreatePostRequestExample.ImageUrl,
            AuthorId = Guid.NewGuid().ToString(),
            Tags = new List<TagRequest>
            {
                new () { Title = SwaggerExamplesConsts.CreateTagRequestExample.Title + "1" },
                new () { Title = SwaggerExamplesConsts.CreateTagRequestExample.Title + "2" },
                new () { Title = SwaggerExamplesConsts.CreateTagRequestExample.Title + "3" },
            },
        };
    }
}