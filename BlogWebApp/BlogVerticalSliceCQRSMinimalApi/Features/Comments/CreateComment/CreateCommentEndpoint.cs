using Asp.Versioning;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Responses;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Comments.CreateComment;

public class CreateCommentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("Comments")
            .MapToApiVersion(1.0)
            .WithTags("Comments");

        group.MapPost(ApiRoutes.CommentsController.CreateComment,
                async ([FromBody] CreateCommentRequest request,
                    [FromServices] ISender sender,
                    [FromServices] HttpContext context) =>
                {
                    var command = request.Adapt<CreateCommentCommand>();

                    var result = await sender.Send(command);

                    var location =
                        $"{context.Request.Scheme}://{context.Request.Host}" +
                        $"{ApiRoutes.CommentsController.Comments}/{result.Id}";

                    return Results.Created(location, new CreatedResponse<int>
                    {
                        Id = result.Id
                    });
                })
            //.RequireAuthorization()
            .WithName("CreateComment")
            .Produces<CreatedResponse<int>>(201)
            .Produces(400)
            .WithSummary("Create comment")
            .WithDescription("Create comment");
    }
}
