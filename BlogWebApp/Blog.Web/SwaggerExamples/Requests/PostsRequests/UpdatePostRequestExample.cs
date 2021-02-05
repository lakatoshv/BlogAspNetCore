using Blog.Contracts.V1.Requests.PostsRequests;
using Blog.Contracts.V1.Requests.TagsRequests;
using Blog.Core.Consts;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Blog.Web.SwaggerExamples.Requests.PostsRequests
{
    /// <summary>
    /// Update post request example.
    /// </summary>
    /// <seealso cref="IExamplesProvider{UpdatePostRequest}" />
    public class UpdatePostRequestExample : IExamplesProvider<UpdatePostRequest>
    {
        /// <inheritdoc cref="IExamplesProvider{T}"/>
        public UpdatePostRequest GetExamples()
        {
            return new UpdatePostRequest
            {
                Id = 1,
                Title = SwaggerExamplesConsts.UpdatePostRequestExample.Title,
                Description = SwaggerExamplesConsts.UpdatePostRequestExample.Description,
                Content = SwaggerExamplesConsts.UpdatePostRequestExample.Content,
                ImageUrl = SwaggerExamplesConsts.UpdatePostRequestExample.ImageUrl,
                AuthorId = Guid.NewGuid().ToString(),
                Seen = 10,
                Likes = 10,
                Dislikes = 10,
                Tags = new List<TagRequest>
                {
                    new TagRequest { Title = SwaggerExamplesConsts.UpdateTagRequestExample.Title + "1" },
                    new TagRequest { Title = SwaggerExamplesConsts.UpdateTagRequestExample.Title + "2" },
                    new TagRequest { Title = SwaggerExamplesConsts.UpdateTagRequestExample.Title + "3" },
                },
            };
        }
    }
}
