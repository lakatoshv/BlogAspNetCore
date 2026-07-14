using Asp.Versioning;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Responses.Chart;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Comments.CommentsActivity;

public class CommentsActivityEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("Comments")
            .MapToApiVersion(1.0)
            .WithTags("Comments");

        group.MapGet("/comments-activity/{userId}", async ([FromRoute] string userId, [FromServices] ISender sender) =>
            {
                var result = await sender.Send(new CommentsActivityQuery());

                return result == null
                    ? Results.NotFound()
                    : Results.Ok(result);
            })
            .WithName("CommentsActivity")
            .Produces<List<ChartDataModel>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Comments Activity")
            .WithDescription("Comments Activity");
    }
}