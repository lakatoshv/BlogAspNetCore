using Blog.Contracts.V1;
using Blog.Contracts.V1.Responses.UsersResponses;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Accounts.Initialize;

public class GetAllEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.AccountsController.Initialize, async ([FromRoute] string userId, [FromServices] ISender sender) =>
            {
                if (string.IsNullOrEmpty(userId))
                    return Results.BadRequest("Something went wrong");

                var result = await sender.Send(new InitializeQuery(UserId: userId));

                return result == null
                    ? Results.NotFound()
                    : Results.Ok(result);
            })
            .WithName("Initialize")
            .Produces<List<RoleResponse>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Initialize")
            .WithDescription("Initialize")
            .HasApiVersion(1.0);
    }
}