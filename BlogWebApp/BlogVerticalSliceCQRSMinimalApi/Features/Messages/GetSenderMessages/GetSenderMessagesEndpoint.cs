using Asp.Versioning;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Responses;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Messages.GetSenderMessages;

public class GetSenderMessagesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("Messages")
            .MapToApiVersion(1.0)
            .WithTags("Messages");

        group.MapGet(ApiRoutes.MessagesController.GetSenderMessages, async ([FromRoute] string senderEmail, [FromServices] ISender sender) =>
            {
                if (string.IsNullOrEmpty(senderEmail))
                    return Results.BadRequest("Something went wrong");

                var result = await sender.Send(new GetSenderMessagesQuery(SenderEmail: senderEmail));

                return result == null
                    ? Results.NotFound()
                    : Results.Ok(result);
            })
            .WithName("GetSenderMessages")
            .Produces<List<MessageResponse>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Get Sender Messages")
            .WithDescription("Get Sender Messages");
    }
}