using Asp.Versioning;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Responses.PostsResponses;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Posts.UserPosts;

public class UserPostsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("Posts")
            .MapToApiVersion(1.0)
            .WithTags("Posts");

        group.MapPost(ApiRoutes.PostsController.UserPosts,
                async ([FromBody] UserPostsRequest request,
                    [FromRoute] string id,
                    [FromServices] ISender sender) =>
                {
                    var command = request.Adapt<UserPostsCommand>();
                    command = command with { Id = id };

                    var result = await sender.Send(command);

                    return result is null
                        ? Results.NotFound()
                        : Results.Ok(result);
                })
            //.RequireAuthorization()
            .WithName("UserPosts")
            .Produces<PagedPostsResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("User Posts")
            .WithDescription("User Posts");
    }
}
