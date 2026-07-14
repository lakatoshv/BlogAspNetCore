using Asp.Versioning;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Responses.PostsResponses;
using Blog.Contracts.V1.Responses.UsersResponses;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Posts.GetAllPosts;

public class GetAllPostsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("Posts")
            .MapToApiVersion(1.0)
            .WithTags("Posts");

        group.MapGet("/posts", async ([FromServices] ISender sender) =>
            {
                var result = await sender.Send(new GetAllPostsQuery());

                return result == null
                    ? Results.NotFound()
                    : Results.Ok(result);
            })
            .WithName("GetAllPosts")
            .Produces<List<PostResponse>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Get All Posts")
            .WithDescription("Get All Posts");
    }
}