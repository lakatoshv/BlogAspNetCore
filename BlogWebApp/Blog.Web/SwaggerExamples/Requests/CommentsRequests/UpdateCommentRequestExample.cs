namespace Blog.Web.SwaggerExamples.Requests.CommentsRequests;

using System;
using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Requests.CommentsRequests;
using Core.Consts;

/// <summary>
/// Update comment request example.
/// </summary>
/// <seealso cref="IExamplesProvider{UpdateCommentRequest}" />
public class UpdateCommentRequestExample : IExamplesProvider<UpdateCommentRequest>
{
    /// <inheritdoc cref="IExamplesProvider{T}"/>
    public UpdateCommentRequest GetExamples()
    {
        return new UpdateCommentRequest
        {
            Id = 0,
            PostId = 0,
            CommentBody = SwaggerExamplesConsts.UpdateCommentRequestExample.CommentBody,
            UserId = Guid.NewGuid().ToString(),
            Likes = 10,
            Dislikes = 10,
        };
    }
}