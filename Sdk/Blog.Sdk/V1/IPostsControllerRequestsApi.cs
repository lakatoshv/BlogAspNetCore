namespace Blog.Sdk.V1
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Blog.Contracts.V1;
    using Blog.Contracts.V1.Requests.PostsRequests;
    using Blog.Contracts.V1.Responses;
    using Blog.Contracts.V1.Responses.PostsResponses;
    using Microsoft.AspNetCore.Mvc;
    using Refit;

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
        [Get(ApiRoutes.PostsController.Posts)]
        Task<ApiResponse<List<PostResponse>>> Index();

        /// <summary>
        /// Gets the posts.
        /// </summary>
        /// <param name="searchParameters">The search parameters.</param>
        /// <returns>Task.</returns>
        /// <response code="200">Get filtered and sorted posts.</response>
        /// <response code="404">Unable to get filtered and sorted posts.</response>
        [Post(ApiRoutes.PostsController.Posts + "/" + ApiRoutes.PostsController.GetPosts)]
        Task<ApiResponse<PagedPostsResponse>> GetPosts([FromBody] PostsSearchParametersRequest searchParameters);

        /// <summary>
        /// Gets the user posts.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="searchParameters">The search parameters.</param>
        /// <returns>Task.</returns>
        /// <response code="200">Get filtered and sorted user posts.</response>
        /// <response code="404">Unable to get filtered and sorted user posts.</response>
        [Post(ApiRoutes.PostsController.Posts + "/" + ApiRoutes.PostsController.UserPosts)]
        Task<ApiResponse<PagedPostsResponse>> GetUserPosts([FromRoute] string id,
            [FromBody] PostsSearchParametersRequest searchParameters);

        /// <summary>
        /// Shows the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task</returns>
        /// <response code="200">Get post by id.</response>
        /// <response code="404">Unable to get post by id.</response>
        [Get(ApiRoutes.PostsController.Posts + "/show")]
        Task<ApiResponse<PostWithPagedCommentsResponse>> Show([FromRoute] int id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>IActionResult.</returns>
        /// <response code="201">Create new post.</response>
        /// <response code="400">Unable to create new post.</response>
        [Post(ApiRoutes.PostsController.Posts)]
        Task<ApiResponse<PostResponse>> CreateAsync([FromBody] CreatePostRequest model);

        /// <summary>
        /// Likes the post asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        /// <response code="204">Likes the post.</response>
        /// <response code="400">Unable to likes the post, model is invalid.</response>
        /// <response code="404">Unable to likes the post, post not found.</response>
        [Put(ApiRoutes.PostsController.Posts + "/like")]
        Task<ApiResponse<PostViewResponse>> LikePostAsync([FromRoute] int id);

        /// <summary>
        /// Dislikes the post asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        /// <response code="204">Dislikes the post.</response>
        /// <response code="400">Unable to dislikes the post, model is invalid.</response>
        /// <response code="404">Unable to dislikes the post, post not found.</response>
        [Put(ApiRoutes.PostsController.Posts + "/dislike")]
        Task<ApiResponse<PostViewResponse>> DislikePostAsync([FromRoute] int id);

        /// <summary>
        /// Edits the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns>Task.</returns>
        /// <response code="204">Edit post by id.</response>
        /// <response code="400">Unable to edit post by id, model is invalid.</response>
        /// <response code="404">Unable to edit post by id, post not found.</response>
        [Put(ApiRoutes.PostsController.Posts)]
        Task<ApiResponse<PostViewResponse>> EditAsync([FromRoute] int id, [FromBody] UpdatePostRequest model);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="authorId">The author identifier.</param>
        /// <returns>Task.</returns>
        /// <response code="200">Delete post by id.</response>
        /// <response code="404">Unable to delete post by id, post not found.</response>
        [Delete(ApiRoutes.PostsController.Posts)]
        Task<ApiResponse<CreatedResponse<int>>> DeleteAsync(int id, string authorId);
    }
}