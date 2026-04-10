using Asp.Versioning;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Responses;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Messages.CreateMessage;

public class CreateMessageEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var group = app.MapGroup(ApiRoutes.MessagesController.Messages)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithTags("Messages");

        group.MapPost("/",
                async ([FromBody] CreateMessageRequest request,
                    [FromServices] ISender sender,
                    [FromServices] HttpContext context) =>
                {
                    var command = request.Adapt<CreateMessageCommand>();

                    var result = await sender.Send(command);

                    var location =
                        $"{context.Request.Scheme}://{context.Request.Host}" +
                        $"/messages/{result.Id}";

                    return Results.Created(location, new CreatedResponse<int>
                    {
                        Id = result.Id
                    });
                })
            //.RequireAuthorization()
            .WithName("CreateMessage")
            .Produces<CreatedResponse<int>>(201)
            .Produces(400)
            .WithSummary("Create message")
            .WithDescription("Create message")
            .HasApiVersion(1.0);
    }
}
