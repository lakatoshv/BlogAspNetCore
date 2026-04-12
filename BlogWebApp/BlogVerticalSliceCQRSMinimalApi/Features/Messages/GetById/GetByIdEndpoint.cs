using Blog.Contracts.V1;
using Blog.Contracts.V1.Responses;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Messages.GetById;

public class GetByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.MessagesController.Show, async ([FromRoute] int id, [FromServices] ISender sender) =>
            {
                var result = await sender.Send(new GetByIdQuery(Id: id));

                return result == null
                    ? Results.NotFound()
                    : Results.Ok(result);
            })
            .WithName("GetById")
            .Produces<MessageResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Get By Id")
            .WithDescription("Get By Id")
            .HasApiVersion(1.0);
    }
}