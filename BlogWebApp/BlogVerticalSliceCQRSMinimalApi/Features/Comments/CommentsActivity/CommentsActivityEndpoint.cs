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
        app.MapGet(ApiRoutes.AccountsController.Initialize, async ([FromRoute] string userId, [FromServices] ISender sender) =>
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
            .WithDescription("Comments Activity")
            .HasApiVersion(1.0);
    }
}