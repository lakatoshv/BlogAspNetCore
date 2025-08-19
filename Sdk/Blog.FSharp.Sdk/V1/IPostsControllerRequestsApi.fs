namespace Blog.FSharp.Sdk.V1

open System.Collections.Generic
open System.Threading.Tasks
open Refit
open Blog.FSharp.Contracts.V1.Responses
open Blog.FSharp.Contracts.V1.Requests.PostsRequests
open Blog.FSharp.Contracts.V1.Responses.PostsResponses

/// <summary>
/// Posts controller requests API interface.
/// </summary>
type IPostsControllerRequestsApi =

    /// <summary>
    /// Indexes this instance.
    /// </summary>
    /// <returns>Task.</returns>
    /// <response code="200">Get all posts.</response>
    /// <response code="404">Unable to get all posts.</response>
    [<Get("/api/v1/posts")>]
    abstract member Index : unit -> Task<List<PostResponse>>

    /// <summary>
    /// Gets the posts.
    /// </summary>
    /// <param name="searchParameters">The search parameters.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Get filtered and sorted posts.</response>
    /// <response code="404">Unable to get filtered and sorted posts.</response>
    [<Post("/api/v1/posts/get-posts")>]
    abstract member GetPosts : searchParameters:PostsSearchParametersRequest -> Task<PagedPostsResponse>

    /// <summary>
    /// Gets the user posts.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="searchParameters">The search parameters.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Get filtered and sorted user posts.</response>
    /// <response code="404">Unable to get filtered and sorted user posts.</response>
    [<Post("/api/v1/posts/user-posts/{id}")>]
    abstract member GetUserPosts : id:string * searchParameters:PostsSearchParametersRequest -> Task<PagedPostsResponse>

    /// <summary>
    /// Shows the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task</returns>
    /// <response code="200">Get post by id.</response>
    /// <response code="404">Unable to get post by id.</response>
    [<Get("/api/v1/posts/show/{id}")>]
    abstract member Show : id:int -> Task<PostWithPagedCommentsResponse>

    /// <summary>
    /// Creates the asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>IActionResult.</returns>
    /// <response code="201">Create new post.</response>
    /// <response code="400">Unable to create new post.</response>
    [<Post("/api/v1/posts")>]
    abstract member CreateAsync : model:CreatePostRequest -> Task<PostResponse>

    /// <summary>
    /// Likes the post asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task.</returns>
    /// <response code="204">Likes the post.</response>
    /// <response code="400">Unable to likes the post, model is invalid.</response>
    /// <response code="404">Unable to likes the post, post not found.</response>
    [<Put("/api/v1/posts/like/{id}")>]
    abstract member LikePostAsync : id:int -> Task<PostViewResponse>

    /// <summary>
    /// Dislikes the post asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task.</returns>
    /// <response code="204">Dislikes the post.</response>
    /// <response code="400">Unable to dislikes the post, model is invalid.</response>
    /// <response code="404">Unable to dislikes the post, post not found.</response>
    [<Put("/api/v1/posts/dislike/{id}")>]
    abstract member DislikePostAsync : id:int -> Task<PostViewResponse>

    /// <summary>
    /// Edits the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="model">The model.</param>
    /// <returns>Task.</returns>
    /// <response code="204">Edit post by id.</response>
    /// <response code="400">Unable to edit post by id, model is invalid.</response>
    /// <response code="404">Unable to edit post by id, post not found.</response>
    [<Put("/api/v1/posts/{id}")>]
    abstract member EditAsync : id:int * model:UpdatePostRequest -> Task<PostViewResponse>

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="authorId">The author identifier.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Delete post by id.</response>
    /// <response code="404">Unable to delete post by id, post not found.</response>
    [<Delete("/api/v1/posts/{id}")>]
    abstract member DeleteAsync : id:int * authorId:string -> Task<CreatedResponse<int>>

