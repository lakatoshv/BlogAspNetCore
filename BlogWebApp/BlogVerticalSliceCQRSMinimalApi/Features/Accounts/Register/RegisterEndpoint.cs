using Asp.Versioning;
using Blog.Contracts.V1;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Accounts.Register;

public class RegisterEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("Accounts")
            .MapToApiVersion(1.0)
            .WithTags("Accounts");

        group.MapPost(ApiRoutes.AccountsController.Register,
                async ([FromBody] RegisterRequest request, [FromServices] ISender sender) =>
                {
                    var command = request.Adapt<RegisterCommand>();

                    var result = await sender.Send(command);

                    if (!result.Succeeded)
                        return Results.BadRequest(result);

                    /*request.Roles ??= ["User"];

                    foreach (var role in model.Roles)
                    {
                        var roleResult = await userManager.AddToRoleAsync(user, role);
                        if (!roleResult.Succeeded)
                            return Results.BadRequest(roleResult);
                    }*/

                    return Results.NoContent();
                })
            //.RequireAuthorization()
            .WithName("Register")
            .Produces(204)
            .Produces<IdentityResult>(400)
            .WithSummary("Register")
            .WithDescription("Register");
    }
}