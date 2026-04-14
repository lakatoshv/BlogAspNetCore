using Blog.Contracts.V1;
using Blog.Contracts.V1.Responses.Chart;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Accounts.UsersActivity;

public class UsersActivityEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiRoutes.AccountsController.UsersActivity, async ([FromServices] ISender sender) =>
            {
                var result = await sender.Send(new UsersActivityQuery());

                return result == null
                    ? Results.NotFound()
                    : Results.Ok(result);
            })
            .WithName("UsersActivity")
            .Produces<List<ChartDataModel>>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Users Activity")
            .WithDescription("Users Activity")
            .HasApiVersion(1.0);
    }
}