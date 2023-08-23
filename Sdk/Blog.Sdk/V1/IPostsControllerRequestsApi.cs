namespace Blog.Sdk.V1;

using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using Blog.Contracts.V1.Requests.PostsRequests;
using Blog.Contracts.V1.Responses;
using Blog.Contracts.V1.Responses.PostsResponses;

/// <summary>
/// Posts controller requests api interface.
/// </summary>
public interface IPostsControllerRequestsApi
{
    /// <summary>
    /// Indexes this instance.
    /// </summary>
    /// <returns>Task.</returns>
    /// <response code="200">Get all posts.</response>
    /// <response code="404">Unable to get all posts.</response>
    [Get("/api/v1/posts")]
    Task<List<PostResponse>> Index();

    /// <summary>
    /// Gets the posts.
    /// </summary>
    /// <param name="searchParameters">The search parameters.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Get filtered and sorted posts.</response>
    /// <response code="404">Unable to get filtered and sorted posts.</response>
    [Post("/api/v1/posts/get-posts")]
    Task<PagedPostsResponse> GetPosts(PostsSearchParametersRequest searchParameters);

    /// <summary>
    /// Gets the user posts.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="searchParameters">The search parameters.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Get filtered and sorted user posts.</response>
    /// <response code="404">Unable to get filtered and sorted user posts.</response>
    [Post("/api/v1/posts/user-posts/{id}")]
    Task<PagedPostsResponse> GetUserPosts(string id,
        PostsSearchParametersRequest searchParameters);

    /// <summary>
    /// Shows the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task</returns>
    /// <response code="200">Get post by id.</response>
    /// <response code="404">Unable to get post by id.</response>
    [Get("/api/v1/posts/show/{id}")]
    Task<PostWithPagedCommentsResponse> Show(int id);

    /// <summary>
    /// Creates the asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>IActionResult.</returns>
    /// <response code="201">Create new post.</response>
    /// <response code="400">Unable to create new post.</response>
    [Post("/api/v1/posts")]
    Task<PostResponse> CreateAsync(CreatePostRequest model);

    /// <summary>
    /// Likes the post asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task.</returns>
    /// <response code="204">Likes the post.</response>
    /// <response code="400">Unable to likes the post, model is invalid.</response>
    /// <response code="404">Unable to likes the post, post not found.</response>
    [Put("/api/v1/posts/like/{id}")]
    Task<PostViewResponse> LikePostAsync(int id);

    /// <summary>
    /// Dislikes the post asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task.</returns>
    /// <response code="204">Dislikes the post.</response>
    /// <response code="400">Unable to dislikes the post, model is invalid.</response>
    /// <response code="404">Unable to dislikes the post, post not found.</response>
    [Put("/api/v1/posts/dislike/{id}")]
    Task<PostViewResponse> DislikePostAsync(int id);

    /// <summary>
    /// Edits the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="model">The model.</param>
    /// <returns>Task.</returns>
    /// <response code="204">Edit post by id.</response>
    /// <response code="400">Unable to edit post by id, model is invalid.</response>
    /// <response code="404">Unable to edit post by id, post not found.</response>
    [Put("/api/v1/posts/{id}")]
    Task<PostViewResponse> EditAsync(int id, UpdatePostRequest model);

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="authorId">The author identifier.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Delete post by id.</response>
    /// <response code="404">Unable to delete post by id, post not found.</response>
    [Delete("/api/v1/posts/{id}")]
    Task<CreatedResponse<int>> DeleteAsync(int id, string authorId);
}