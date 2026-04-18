using Asp.Versioning;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Responses.PostsResponses;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Posts.GetPosts;

public class UserPostsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("Posts")
            .MapToApiVersion(1.0)
            .WithTags("Posts");

        group.MapPost("",
                async ([FromBody] GetPostsRequest request,
                    [FromServices] ISender sender) =>
                {
                    var command = request.Adapt<GetPostsCommand>();

                    var result = await sender.Send(command);

                    return result is null
                        ? Results.NotFound()
                        : Results.Ok(result);
                })
            //.RequireAuthorization()
            .WithName("GetPosts")
            .Produces<PagedPostsResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Get Posts")
            .WithDescription("Get Posts");
    }
}
