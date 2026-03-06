using Asp.Versioning;
using AutoMapper;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Requests.MessagesRequests;
using Blog.Contracts.V1.Responses;
using Blog.Data.Models;
using Blog.Data.Specifications;
using Blog.EntityServices.Interfaces;

namespace BlogMinimalApi.ApiEndpoints;

/// <summary>
/// Messages Api endpoints.
/// </summary>
public class MessagesApiEndpoints : IRoutesInstaller
{
    public void InstallApiRoutes(WebApplication app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var group = app.MapGroup(ApiRoutes.MessagesController.Messages)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithTags("Messages");

        // -------------------------
        // PUBLIC ENDPOINTS
        // -------------------------

        var publicGroup = group.AllowAnonymous();

        // GET ALL
        publicGroup.MapGet("",
                async (IMessagesService messagesService,
                    IMapper mapper) =>
                {
                    var messages = await messagesService.GetAllAsync();

                    if (messages is null || messages.Count == 0)
                        return Results.NotFound();

                    var response = mapper.Map<List<MessageResponse>>(messages);

                    return Results.Ok(response);
                })
            .Produces<List<MessageResponse>>()
            .Produces(404);


        // GET BY RECIPIENT
        publicGroup.MapGet(ApiRoutes.MessagesController.GetRecipientMessages,
                async (string recipientId,
                    IMessagesService messagesService,
                    IMapper mapper) =>
                {
                    var messages = await messagesService.GetAllAsync(
                        new MessageSpecification(x =>
                            x.RecipientId.Equals(recipientId, StringComparison.CurrentCultureIgnoreCase)));

                    if (messages is null || messages.Count == 0)
                        return Results.NotFound();

                    return Results.Ok(mapper.Map<List<MessageResponse>>(messages));
                })
            .Produces<List<MessageResponse>>()
            .Produces(404);


        // GET BY SENDER
        publicGroup.MapGet(ApiRoutes.MessagesController.GetSenderMessages,
                async (string senderEmail,
                    IMessagesService messagesService,
                    IMapper mapper) =>
                {
                    var messages = await messagesService.GetAllAsync(
                        new MessageSpecification(x =>
                            x.SenderEmail.Equals(senderEmail, StringComparison.CurrentCultureIgnoreCase)));

                    if (messages is null || messages.Count == 0)
                        return Results.NotFound();

                    return Results.Ok(mapper.Map<List<MessageResponse>>(messages));
                })
            .Produces<List<MessageResponse>>()
            .Produces(404);


        // GET BY ID
        publicGroup.MapGet(ApiRoutes.MessagesController.Show,
                async (int id,
                    IMessagesService messagesService,
                    IMapper mapper) =>
                {
                    var message = await messagesService.FindAsync(id);

                    return message is null
                        ? Results.NotFound()
                        : Results.Ok(mapper.Map<MessageResponse>(message));
                })
            .Produces<MessageResponse>()
            .Produces(404);


        // -------------------------
        // PRIVATE ENDPOINTS
        // -------------------------

        var privateGroup = group.RequireAuthorization();

        // CREATE
        privateGroup.MapPost("",
                async (CreateMessageRequest request,
                    IMessagesService messagesService,
                    IMapper mapper,
                    HttpContext context) =>
                {
                    var userId = context.User.FindFirst("sub")?.Value;

                    if (string.IsNullOrEmpty(userId))
                        return Results.BadRequest("Unauthorized");

                    request.SenderId = userId;

                    var message = mapper.Map<Message>(request);

                    await messagesService.InsertAsync(message);

                    return Results.Created($"/messages/{message.Id}", null);
                })
            .Produces(201)
            .Produces(400);


        // UPDATE
        privateGroup.MapPut("{id:int}",
                async (int id,
                    UpdateMessageRequest request,
                    IMessagesService messagesService,
                    IMapper mapper,
                    HttpContext context) =>
                {
                    var userId = context.User.FindFirst("sub")?.Value;

                    if (string.IsNullOrEmpty(userId))
                        return Results.BadRequest("Unauthorized");

                    var message = await messagesService.FindAsync(id);

                    if (message is null)
                        return Results.NotFound();

                    if (message.SenderId != userId &&
                        message.RecipientId != userId)
                    {
                        return Results.BadRequest(
                            new { ErrorMessage = "You are not author or recipient." });
                    }

                    mapper.Map(request, message);

                    await messagesService.UpdateAsync(message);

                    return Results.NoContent();
                })
            .Produces(204)
            .Produces(400)
            .Produces(404);


        // DELETE
        privateGroup.MapDelete("{id:int}",
                async (int id,
                    IMessagesService messagesService,
                    HttpContext context) =>
                {
                    var userId = context.User.FindFirst("sub")?.Value;

                    if (string.IsNullOrEmpty(userId))
                        return Results.BadRequest("Unauthorized");

                    var message = await messagesService.FindAsync(id);

                    if (message is null)
                        return Results.NotFound();

                    if (message.SenderId != userId &&
                        message.RecipientId != userId)
                    {
                        return Results.BadRequest(
                            new { ErrorMessage = "You are not allowed." });
                    }

                    await messagesService.DeleteAsync(message);

                    return Results.Ok(new CreatedResponse<int> { Id = id });
                })
            .Produces<CreatedResponse<int>>()
            .Produces(404)
            .Produces(400);
    }
}