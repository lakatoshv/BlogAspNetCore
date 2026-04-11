using Blog.Contracts.V1;
using Blog.Contracts.V1.Responses.PostsResponses;
using Blog.Contracts.V1.Responses.UsersResponses;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Posts.GetPostById;

public class GetPostByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.PostsController.Show, async ([FromRoute] int id, [FromServices] ISender sender) =>
            {
                var result = await sender.Send(new GetPostByIdQuery(Id: id));

                return result == null
                    ? Results.NotFound()
                    : Results.Ok(result);
            })
            .WithName("GetPostById")
            .Produces<PostWithPagedCommentsResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Get Post By Id")
            .WithDescription("Get Post By Id")
            .HasApiVersion(1.0);
    }
}