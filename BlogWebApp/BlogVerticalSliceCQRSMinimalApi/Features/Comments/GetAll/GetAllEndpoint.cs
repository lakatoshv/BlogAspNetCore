using Blog.Contracts.V1.Responses.CommentsResponses;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Comments.GetAll;

public class GetAllEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/comments", async ([FromRoute] string userId, [FromServices] ISender sender) =>
            {
                var result = await sender.Send(new GetAllQuery());

                return result == null
                    ? Results.NotFound()
                    : Results.Ok(result);
            })
            .WithName("GetAll")
            .Produces<List<CommentResponse>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Get All")
            .WithDescription("Get All")
            .HasApiVersion(1.0);
    }
}