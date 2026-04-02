using Asp.Versioning;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Responses;
using Carter;
using Mapster;
using MediatR;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Posts.CreatePost;

public class CreatePostEndpoint : ICarterModule
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

        group.MapPost("/",
                async (CreatePostRequest request,
                    ISender sender,
                    HttpContext context) =>
                {
                    var command = request.Adapt<CreatePostCommand>();

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
            .WithDescription("Create post")
            .HasApiVersion(1.0);
    }
}
