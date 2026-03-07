using Asp.Versioning;
using AutoMapper;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Requests;
using Blog.Contracts.V1.Requests.CommentsRequests;
using Blog.Contracts.V1.Responses;
using Blog.Contracts.V1.Responses.Chart;
using Blog.Contracts.V1.Responses.CommentsResponses;
using Blog.Data.Models;
using Blog.EntityServices.Interfaces;
using Blog.Services.Core.Dtos;

namespace BlogMinimalApi.ApiEndpoints;

/// <summary>
/// Comments Api endpoints.
/// </summary>
public class CommentsApiEndpoints : IRoutesInstaller
{
    public void InstallApiRoutes(WebApplication app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var group = app.MapGroup(ApiRoutes.CommentsController.Comments)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithTags("Comments");

        // -------------------------
        // PUBLIC
        // -------------------------

        var publicGroup = group.AllowAnonymous();

        // Get All Comments
        publicGroup.MapGet("/",
            async (ICommentsService service, IMapper mapper) =>
            {
                var comments = await service.GetAllAsync();
                var response = mapper.Map<List<CommentResponse>>(comments);

                return Results.Ok(response);
            })
            .Produces<List<CommentResponse>>();


        // Comments Activity
        publicGroup.MapGet(ApiRoutes.CommentsController.CommentsActivity,
            async (ICommentsService service) =>
            {
                var activity = await service.GetCommentsActivity();

                return activity is null
                    ? Results.NotFound()
                    : Results.Ok(activity);
            })
            .Produces<ChartDataModel>()
            .Produces(404);


        // Get Paged Comments
        publicGroup.MapPost(ApiRoutes.CommentsController.GetCommentsByFilter,
            async (SortParametersRequest? sortParameters,
                   ICommentsService service,
                   IMapper mapper) =>
            {
                sortParameters ??= new SortParametersRequest();
                sortParameters.CurrentPage ??= 1;
                sortParameters.PageSize = 10;

                var result = await service.GetPagedComments(
                    mapper.Map<SortParametersDto>(sortParameters));

                return result is null
                    ? Results.NotFound()
                    : Results.Ok(mapper.Map<PagedCommentsResponse>(result));
            })
            .Produces<PagedCommentsResponse>()
            .Produces(404);


        // Get Comments By Post
        publicGroup.MapPost(ApiRoutes.CommentsController.GetCommentsByPost,
            async (int id,
                   SortParametersRequest? sortParameters,
                   ICommentsService service,
                   IMapper mapper) =>
            {
                sortParameters ??= new SortParametersRequest();
                sortParameters.CurrentPage ??= 1;
                sortParameters.PageSize = 10;

                var result = await service.GetPagedCommentsByPostId(
                    id,
                    mapper.Map<SortParametersDto>(sortParameters));

                return result is null
                    ? Results.NotFound()
                    : Results.Ok(mapper.Map<PagedCommentsResponse>(result));
            })
            .Produces<PagedCommentsResponse>()
            .Produces(404);


        // Get Comment By ID
        publicGroup.MapGet("/{id:int}",
            async (int id,
                   ICommentsService service,
                   IMapper mapper) =>
            {
                var comment = await service.GetCommentAsync(id);

                return comment is null
                    ? Results.NotFound()
                    : Results.Ok(mapper.Map<CommentResponse>(comment));
            })
            .WithName(ApiRoutes.CommentsController.GetComment)
            .Produces<CommentResponse>()
            .Produces(404);

        // -------------------------
        // PRIVATE (Authorized)
        // -------------------------

        var privateGroup = group.RequireAuthorization();

        // Create Comment
        privateGroup.MapPost(ApiRoutes.CommentsController.CreateComment,
            async (CreateCommentRequest request,
                   ICommentsService service,
                   IMapper mapper,
                   HttpContext context) =>
            {
                var comment = mapper.Map<Comment>(request);
                comment.CreatedAt = DateTime.UtcNow;

                await service.InsertAsync(comment);

                var location =
                    $"{context.Request.Scheme}://{context.Request.Host}" +
                    $"{ApiRoutes.CommentsController.Comments}/{comment.Id}";

                return Results.Created(location, new CreatedResponse<int>
                {
                    Id = comment.Id
                });
            })
            .Produces<CreatedResponse<int>>(201)
            .Produces(400);


        // Edit Comment
        privateGroup.MapPut(ApiRoutes.CommentsController.EditComment,
            async (int id,
                   UpdateCommentRequest request,
                   ICommentsService service,
                   IMapper mapper) =>
            {
                var origin = await service.GetCommentAsync(id);

                if (origin is null || !origin.UserId.Equals(request.UserId))
                    return Results.NotFound();

                request.CreatedAt = DateTime.UtcNow;

                var updated = mapper.Map(request, origin);

                await service.UpdateAsync(updated);

                var refreshed = await service.GetCommentAsync(id);

                return Results.Ok(mapper.Map<CommentResponse>(refreshed));
            })
            .Produces<CommentResponse>()
            .Produces(404);


        // Delete Comment
        privateGroup.MapDelete(ApiRoutes.CommentsController.DeleteComment,
            async (int id,
                   ICommentsService service) =>
            {
                var comment = await service.GetCommentAsync(id);

                if (comment is null)
                    return Results.NotFound();

                await service.DeleteAsync(comment);

                return Results.Ok(new CreatedResponse<int> { Id = id });
            })
            .Produces<CreatedResponse<int>>()
            .Produces(404);
    }
}