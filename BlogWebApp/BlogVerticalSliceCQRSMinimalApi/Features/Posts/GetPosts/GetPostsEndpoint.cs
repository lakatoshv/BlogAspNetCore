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
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var group = app.MapGroup(ApiRoutes.PostsController.Posts)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithTags("Posts");

        group.MapPost(ApiRoutes.PostsController.GetPosts,
                async ([FromBody] UserPostsRequest request,
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
            .WithDescription("Get Posts")
            .HasApiVersion(1.0);
    }
}
