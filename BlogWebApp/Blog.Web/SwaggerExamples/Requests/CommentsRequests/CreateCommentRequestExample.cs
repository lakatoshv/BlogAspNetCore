using System;
using Blog.Contracts.V1.Requests.CommentsRequests;
using Blog.Core.Consts;
using Swashbuckle.AspNetCore.Filters;

namespace Blog.Web.SwaggerExamples.Requests.CommentsRequests
{
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
}