using Asp.Versioning;
using Blog.Contracts.V1;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Accounts.Login;

public class LoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("Accounts")
            .MapToApiVersion(1.0)
            .WithTags("Accounts");

        group.MapPost(ApiRoutes.AccountsController.Login,
                async ([FromBody] LoginRequest request, [FromServices] ISender sender) =>
                {
                    if (request is null)
                        return Results.BadRequest();

                    var command = request.Adapt<LoginCommand>();

                    var result = await sender.Send(command);

                    return result is null
                        ? Results.BadRequest("Invalid name or password")
                        : Results.Ok(result);
                })
            //.RequireAuthorization()
            .WithName("Login")
            .Produces(200)
            .Produces<string>(400)
            .WithSummary("Login")
            .WithDescription("Login");
    }
}