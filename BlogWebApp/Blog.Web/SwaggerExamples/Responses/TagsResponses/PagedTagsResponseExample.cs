namespace Blog.Web.SwaggerExamples.Responses.TagsResponses;

using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Responses;
using Blog.Contracts.V1.Responses.TagsResponses;
using Core.Consts;

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