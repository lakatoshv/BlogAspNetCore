using Asp.Versioning;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Responses;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Messages.GetRecipientMessages;

public class GetRecipientMessagesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("Messages")
            .MapToApiVersion(1.0)
            .WithTags("Messages");

        group.MapGet(ApiRoutes.MessagesController.GetRecipientMessages, async ([FromRoute] string recipientId, [FromServices] ISender sender) =>
            {
                if (string.IsNullOrEmpty(recipientId))
                    return Results.BadRequest("Something went wrong");

                var result = await sender.Send(new GetRecipientMessagesQuery(RecipientId: recipientId));

                return result == null
                    ? Results.NotFound()
                    : Results.Ok(result);
            })
            .WithName("GetRecipientMessages")
            .Produces<List<MessageResponse>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Get Recipient Messages")
            .WithDescription("Get Recipient Messages");
    }
}