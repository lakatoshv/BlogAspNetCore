using Asp.Versioning;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Responses.CommentsResponses;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Comments.GetCommentsByFilter;

public class GetCommentsByFilterEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("Comments")
            .MapToApiVersion(1.0)
            .WithTags("Comments");

        group.MapPost(ApiRoutes.CommentsController.GetCommentsByFilter,
                async ([FromServices] GetCommentsByFilterRequest request,
                    ISender sender) =>
                {
                    var command = request.Adapt<GetCommentsByFilterCommand>();

                    var result = await sender.Send(command);

                    return result is null
                        ? Results.NotFound()
                        : Results.Ok(result);
                })
            //.RequireAuthorization()
            .WithName("GetCommentsByFilter")
            .Produces<PagedCommentsResponse>()
            .Produces(400)
            .WithSummary("Get Comments By Filter")
            .WithDescription("Get CommentsBy Filter");
    }
}
