using Asp.Versioning;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Responses.CommentsResponses;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Comments.GetCommentsByPost;

public class GetCommentsByPostEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var group = app.MapGroup(ApiRoutes.CommentsController.Comments)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithTags("Comments");

        group.MapPost(ApiRoutes.CommentsController.GetCommentsByPost,
                async ([FromRoute] int id, [FromBody] GetCommentsByPostRequest request,
                    [FromServices] ISender sender) =>
                {
                    var command = request.Adapt<GetCommentsByPostCommand>();
                    command = command with { PostId = id };

                    var result = await sender.Send(command);

                    return result is null
                        ? Results.NotFound()
                        : Results.Ok(result);
                })
            //.RequireAuthorization()
            .WithName("Get Comments By Post")
            .Produces<PagedCommentsResponse>()
            .Produces(400)
            .WithSummary("Get Comments By Post")
            .WithDescription("Get Comments By Post")
            .HasApiVersion(1.0);
    }
}