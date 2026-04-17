using Asp.Versioning;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Responses.CommentsResponses;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Comments.GetAll;

/*public class GetAllEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/сomments")
            .MapToApiVersion(1.0)
            .WithTags("Comments");

        group.MapGet("", async ([FromRoute] string userId, [FromServices] ISender sender) =>
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
            .WithDescription("Get All");
    }
}*/