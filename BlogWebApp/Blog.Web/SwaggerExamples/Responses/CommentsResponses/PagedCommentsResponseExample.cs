namespace Blog.Web.SwaggerExamples.Responses.CommentsResponses;

using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Responses;
using Blog.Contracts.V1.Responses.CommentsResponses;
using Core.Consts;

/// <summary>
/// Paged comments response example.
/// </summary>
/// <seealso cref="IExamplesProvider{PagedCommentsResponse}" />
public class PagedCommentsResponseExample : IExamplesProvider<PagedCommentsResponse>
{
    /// <inheritdoc cref="IExamplesProvider{T}"/>
    public PagedCommentsResponse GetExamples()
    {
        return new PagedCommentsResponse
        {
            Comments = new List<CommentResponse> {
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
        };
    }
}