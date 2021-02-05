using Blog.Contracts.V1.Responses;
using Blog.Contracts.V1.Responses.TagsResponses;
using Blog.Core.Consts;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

namespace Blog.Web.SwaggerExamples.Responses.TagsResponses
{
    /// <summary>
    /// Paged tags response example.
    /// </summary>
    /// <seealso cref="IExamplesProvider{PagedTagsResponse}" />
    public class PagedTagsResponseExample : IExamplesProvider<PagedTagsResponse>
    {
        /// <inheritdoc cref="IExamplesProvider{T}"/>
        public PagedTagsResponse GetExamples()
        {
            return new PagedTagsResponse
            {
                PageInfo = new PageInfoResponse
                {
                    PageNumber = 1,
                    PageSize = 10,
                    TotalItems = 100,
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
                    
                    new TagResponse
                    {
                        Id = 0,
                        Title = SwaggerExamplesConsts.TagResponseExample.Title + "3",
                    }
                },
            };
        }
    }
}
