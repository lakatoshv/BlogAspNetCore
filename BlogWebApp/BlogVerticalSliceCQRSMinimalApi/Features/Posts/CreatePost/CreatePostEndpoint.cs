using Asp.Versioning;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Responses;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Posts.CreatePost;

public class GetPostsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("Posts")
            .MapToApiVersion(1.0)
            .WithTags("Posts");

        group.MapPost("/",
                async ([FromBody] GetPostsRequest request,
                    [FromServices] ISender sender,
                    [FromServices] HttpContext context) =>
                {
                    var command = request.Adapt<GetPostsCommand>();

                    var result = await sender.Send(command);

                    var location =
                        $"{context.Request.Scheme}://{context.Request.Host}" +
                        $"/posts/{result.Id}";

                    return Results.Created(location, new CreatedResponse<int>
                    {
                        Id = result.Id
                    });
                })
            //.RequireAuthorization()
            .WithName("CreatePost")
            .Produces<CreatedResponse<int>>(201)
            .Produces(400)
            .WithSummary("Create post")
            .WithDescription("Create post");
    }
}
