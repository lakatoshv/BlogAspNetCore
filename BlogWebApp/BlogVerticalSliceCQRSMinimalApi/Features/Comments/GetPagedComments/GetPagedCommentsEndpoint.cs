using Asp.Versioning;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Responses.CommentsResponses;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Comments.GetPagedComments;

public class GetCommentsByFilterEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var group = app.MapGroup(ApiRoutes.CommentsController.Comments)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithTags("Comments");

        group.MapPost(ApiRoutes.CommentsController.GetCommentsByFilter,
                async ([FromBody] GetCommentsByFilterRequest request,
                    [FromServices] ISender sender) =>
                {
                    var command = request.Adapt<GetPagedCommentsCommand>();

                    var result = await sender.Send(command);

                    return result is null
                        ? Results.NotFound()
                        : Results.Ok(result);
                })
            //.RequireAuthorization()
            .WithName("GetPagedComments")
            .Produces<PagedCommentsResponse>()
            .Produces(400)
            .WithSummary("Get Paged Comments")
            .WithDescription("Get Paged Comments")
            .HasApiVersion(1.0);
    }
}
