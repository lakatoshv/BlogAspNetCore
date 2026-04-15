using Blog.Contracts.V1.Responses.CommentsResponses;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Comments.GetCommentById;

public class GetCommentByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/comments/{id:int}", async ([FromRoute] int id, [FromServices] ISender sender) =>
            {
                var result = await sender.Send(new GetCommentByIdQuery(Id: id));

                return result == null
                    ? Results.NotFound()
                    : Results.Ok(result);
            })
            .WithName("GetCommentById")
            .Produces<CommentResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Get Comment By Id")
            .WithDescription("Get Comment By Id")
            .HasApiVersion(1.0);
    }
}