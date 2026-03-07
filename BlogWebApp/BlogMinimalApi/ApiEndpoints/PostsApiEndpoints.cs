using Asp.Versioning;
using AutoMapper;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Requests.PostsRequests;
using Blog.Contracts.V1.Responses;
using Blog.Contracts.V1.Responses.Chart;
using Blog.Contracts.V1.Responses.PostsResponses;
using Blog.Contracts.V1.Responses.UsersResponses;
using Blog.Data.Models;
using Blog.EntityServices.Interfaces;
using Blog.Services.Core.Dtos;
using Blog.Services.Core.Dtos.Posts;

namespace BlogMinimalApi.ApiEndpoints;

/// <summary>
/// Posts Api endpoints.
/// </summary>
public class PostsApiEndpoints : IRoutesInstaller
{
    public void InstallApiRoutes(WebApplication app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var group = app.MapGroup(ApiRoutes.PostsController.Posts)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithTags("Posts");

        // -------------------------
        // PUBLIC ENDPOINTS
        // -------------------------

        var publicGroup = group.AllowAnonymous();

        // GET ALL POSTS
        publicGroup.MapGet("",
                async (IPostsService postsService,
                    IMapper mapper) =>
                {
                    var posts = await postsService.GetAllAsync();

                    if (posts is null || !posts.Any())
                        return Results.NotFound();

                    return Results.Ok(
                        mapper.Map<List<PostResponse>>(posts));
                })
            .Produces<List<PostResponse>>()
            .Produces(404);


        // POSTS ACTIVITY
        publicGroup.MapGet(ApiRoutes.PostsController.PostsActivity,
                async (IPostsService postsService) =>
                {
                    var activity = await postsService.GetPostsActivity();

                    return activity is null
                        ? Results.NotFound()
                        : Results.Ok(activity);
                })
            .Produces<ChartDataModel>()
            .Produces(404);


        // FILTERED POSTS
        publicGroup.MapPost(ApiRoutes.PostsController.GetPosts,
                async (PostsSearchParametersRequest request,
                    IPostsService postsService,
                    IMapper mapper) =>
                {
                    request.SortParameters ??= new();
                    request.SortParameters.OrderBy ??= "asc";
                    request.SortParameters.SortBy ??= "Title";
                    request.SortParameters.CurrentPage ??= 1;
                    request.SortParameters.PageSize ??= 10;

                    var posts = await postsService.GetPostsAsync(
                        mapper.Map<PostsSearchParametersDto>(request));

                    return posts is null
                        ? Results.NotFound()
                        : Results.Ok(mapper.Map<PagedPostsResponse>(posts));
                })
            .Produces<PagedPostsResponse>()
            .Produces(404);


        // USER POSTS
        publicGroup.MapPost(ApiRoutes.PostsController.UserPosts,
                async (string id,
                    PostsSearchParametersRequest request,
                    IPostsService postsService,
                    IMapper mapper) =>
                {
                    request.SortParameters ??= new();
                    request.SortParameters.OrderBy ??= "asc";
                    request.SortParameters.SortBy ??= "Title";
                    request.SortParameters.CurrentPage ??= 1;
                    request.SortParameters.PageSize = 10;

                    var posts = await postsService.GetUserPostsAsync(
                        id,
                        mapper.Map<PostsSearchParametersDto>(request));

                    return posts is null
                        ? Results.NotFound()
                        : Results.Ok(mapper.Map<PagedPostsResponse>(posts));
                })
            .Produces<PagedPostsResponse>()
            .Produces(404);


        // GET POST BY ID
        publicGroup.MapGet(ApiRoutes.PostsController.Show,
                async (int id,
                    IPostsService postsService,
                    IMapper mapper) =>
                {
                    var sort = new SortParametersDto
                    {
                        CurrentPage = 1,
                        PageSize = 10
                    };

                    var post = await postsService.GetPost(id, sort);

                    return post is null
                        ? Results.NotFound()
                        : Results.Ok(
                            mapper.Map<PostWithPagedCommentsResponse>(post));
                })
            .Produces<PostWithPagedCommentsResponse>()
            .Produces(404);


        // -------------------------
        // PRIVATE ENDPOINTS
        // -------------------------

        var privateGroup = group.RequireAuthorization();

        // CREATE POST
        privateGroup.MapPost("",
                async (CreatePostRequest model,
                    IPostsService postsService,
                    IMapper mapper,
                    HttpContext context) =>
                {
                    var userId = context.User.FindFirst("sub")?.Value;

                    if (string.IsNullOrEmpty(userId))
                        return Results.BadRequest("Unauthorized");

                    model.AuthorId = userId;

                    var post = mapper.Map<Post>(model);
                    var tags = mapper.Map<List<Tag>>(model.Tags.Distinct());

                    await postsService.InsertAsync(post, tags);

                    return Results.Created(
                        $"{ApiRoutes.PostsController.Posts}/{post.Id}",
                        new CreatedResponse<int> { Id = post.Id });
                })
            .Produces<CreatedResponse<int>>(201)
            .Produces(400);


        // LIKE POST
        privateGroup.MapPut(ApiRoutes.PostsController.LikePost,
                async (int id,
                    IPostsService postsService,
                    IMapper mapper,
                    HttpContext context) =>
                {
                    var userId = context.User.FindFirst("sub")?.Value;
                    if (string.IsNullOrEmpty(userId))
                        return Results.BadRequest("Unauthorized");

                    var post = await postsService.GetPostAsync(id);
                    if (post is null)
                        return Results.NotFound();

                    post.Likes++;
                    await postsService.UpdateAsync(post);

                    var updated = await postsService.GetPostAsync(id);

                    var response = mapper.Map<PostViewResponse>(updated);
                    response.Author =
                        mapper.Map<ApplicationUserResponse>(updated.Author);

                    return Results.Ok(response);
                })
            .Produces<PostViewResponse>()
            .Produces(404)
            .Produces(400);


        // DISLIKE POST
        privateGroup.MapPut(ApiRoutes.PostsController.DislikePost,
                async (int id,
                    IPostsService postsService,
                    IMapper mapper,
                    HttpContext context) =>
                {
                    var userId = context.User.FindFirst("sub")?.Value;
                    if (string.IsNullOrEmpty(userId))
                        return Results.BadRequest("Unauthorized");

                    var post = await postsService.GetPostAsync(id);
                    if (post is null)
                        return Results.NotFound();

                    post.Dislikes++;
                    await postsService.UpdateAsync(post);

                    var updated = await postsService.GetPostAsync(id);

                    var response = mapper.Map<PostViewResponse>(updated);
                    response.Author =
                        mapper.Map<ApplicationUserResponse>(updated.Author);

                    return Results.Ok(response);
                })
            .Produces<PostViewResponse>()
            .Produces(404)
            .Produces(400);


        // EDIT POST
        privateGroup.MapPut("{id:int}",
                async (int id,
                    UpdatePostRequest model,
                    IPostsService postsService,
                    IPostsTagsRelationsService tagsService,
                    IMapper mapper,
                    HttpContext context) =>
                {
                    var userId = context.User.FindFirst("sub")?.Value;
                    if (string.IsNullOrEmpty(userId))
                        return Results.BadRequest("Unauthorized");

                    var post = await postsService.GetPostAsync(id);
                    if (post is null)
                        return Results.NotFound();

                    if (post.AuthorId != userId)
                        return Results.BadRequest(
                            new { ErrorMessage = "You are not the author." });

                    mapper.Map(model, post);

                    await postsService.UpdateAsync(post);

                    var tags = mapper.Map<List<Tag>>(model.Tags);

                    await tagsService.AddTagsToExistingPost(
                        post.Id,
                        post.PostsTagsRelations.ToList(),
                        tags);

                    var updated = await postsService.GetPostAsync(id);

                    return Results.Ok(
                        mapper.Map<PostViewResponse>(updated));
                })
            .Produces<PostViewResponse>()
            .Produces(404)
            .Produces(400);


        // DELETE POST
        privateGroup.MapDelete("{id:int}",
                async (int id,
                    IPostsService postsService,
                    HttpContext context) =>
                {
                    var userId = context.User.FindFirst("sub")?.Value;
                    if (string.IsNullOrEmpty(userId))
                        return Results.BadRequest("Unauthorized");

                    var post = await postsService.GetPostAsync(id);
                    if (post is null)
                        return Results.NotFound();

                    if (post.AuthorId != userId)
                        return Results.BadRequest(
                            new { ErrorMessage = "You are not the author." });

                    await postsService.DeleteAsync(post);

                    return Results.Ok(
                        new CreatedResponse<int> { Id = id });
                })
            .Produces<CreatedResponse<int>>()
            .Produces(404)
            .Produces(400);
    }
}