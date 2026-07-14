using Asp.Versioning;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Responses.Chart;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Posts.PostsActivity;
/*
public class PostsActivityEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("Posts")
   .MapToApiVersion(1.0)
   .WithTags("Posts");

        group.MapGet(ApiRoutes.PostsController.PostsActivity, async ([FromServices] ISender sender) =>
            {
                var result = await sender.Send(new PostsActivityQuery());

                return result == null
                    ? Results.NotFound()
                    : Results.Ok(result);
            })
            .WithName("PostsActivity")
            .Produces<List<ChartDataModel>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Posts Activity")
            .WithDescription("Posts Activity");
    }
}*/