using Blog.Contracts.V1;
using Blog.Contracts.V1.Responses.UsersResponses;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Accounts.GetAllUsers;

public class GetAllUsersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.AccountsController.GetAllUsers, async ([FromServices] ISender sender) =>
            {
                var result = await sender.Send(new GetAllUsersQuery());

                return result == null
                    ? Results.NotFound()
                    : Results.Ok(result);
            })
            .WithName("GetAllUsers")
            .Produces<List<AccountResponse>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Get All Users")
            .WithDescription("Get All Users")
            .HasApiVersion(1.0);
    }
}