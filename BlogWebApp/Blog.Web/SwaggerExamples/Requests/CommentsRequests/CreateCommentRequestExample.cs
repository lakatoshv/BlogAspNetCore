namespace Blog.Web.SwaggerExamples.Requests.CommentsRequests;

using System;
using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Requests.CommentsRequests;
using Core.Consts;

/// <summary>
/// Create comment request example.
/// </summary>
/// <seealso cref="IExamplesProvider{CreateCommentRequest}" />
public class CreateCommentRequestExample : IExamplesProvider<CreateCommentRequest>
{
    /// <inheritdoc cref="IExamplesProvider{T}"/>
    public CreateCommentRequest GetExamples()
    {
        return new CreateCommentRequest
        {
            PostId = 0,
            CommentBody = SwaggerExamplesConsts.CreateCommentRequestExample.CommentBody,
            UserId = Guid.NewGuid().ToString(),
        };
    }
}