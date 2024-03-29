﻿namespace Blog.Web.SwaggerExamples.Responses.CommentsResponses;

using System;
using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Responses.CommentsResponses;
using Core.Consts;

/// <summary>
/// Comment response example.
/// </summary>
/// <seealso cref="IExamplesProvider{CommentResponse}" />
public class CommentResponseExample : IExamplesProvider<CommentResponse>
{
    /// <inheritdoc cref="IExamplesProvider{T}"/>
    public CommentResponse GetExamples()
    {
        return new CommentResponse
        {
            Id = 0,
            PostId = 0,
            CommentBody = SwaggerExamplesConsts.CommentResponseExample.CommentBody,
            CreatedAt = DateTime.Now,
            UserId = Guid.NewGuid().ToString(),
        };
    }
}