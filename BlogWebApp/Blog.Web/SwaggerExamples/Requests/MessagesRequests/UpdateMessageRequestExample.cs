namespace Blog.Web.SwaggerExamples.Requests.MessagesRequests;

using System;
using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Requests.MessagesRequests;
using Core.Consts;

/// <summary>
/// Update message request example.
/// </summary>
/// <seealso cref="IExamplesProvider{UpdateMessageRequest}" />
public class UpdateMessageRequestExample : IExamplesProvider<UpdateMessageRequest>
{
    /// <inheritdoc cref="IExamplesProvider{T}"/>
    public UpdateMessageRequest GetExamples()
    {
        return new UpdateMessageRequest
        {
            SenderId = Guid.NewGuid().ToString(),
            RecipientId = Guid.NewGuid().ToString(),
            Subject = SwaggerExamplesConsts.UpdateMessageRequestExample.Subject,
            Body = SwaggerExamplesConsts.UpdateMessageRequestExample.Body,
            MessageType = 0,
        };
    }
}