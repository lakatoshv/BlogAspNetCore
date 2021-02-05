using System;
using Blog.Contracts.V1.Requests.MessagesRequests;
using Blog.Core.Consts;
using Swashbuckle.AspNetCore.Filters;

namespace Blog.Web.SwaggerExamples.Requests.MessagesRequests
{
    /// <summary>
    /// Create message request example.
    /// </summary>
    /// <seealso cref="IExamplesProvider{CreateMessageRequest}" />
    public class CreateMessageRequestExample : IExamplesProvider<CreateMessageRequest>
    {
        /// <inheritdoc cref="IExamplesProvider{T}"/>
        public CreateMessageRequest GetExamples() =>
            new CreateMessageRequest
            {
                SenderId = Guid.NewGuid().ToString(),
                RecipientId = Guid.NewGuid().ToString(),
                Subject = SwaggerExamplesConsts.CreateMessageRequestExample.Subject,
                Body = SwaggerExamplesConsts.CreateMessageRequestExample.Body,
                MessageType = 0,
            };
    }
}