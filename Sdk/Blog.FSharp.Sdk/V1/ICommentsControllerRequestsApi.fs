namespace Blog.FSharp.Sdk.V1

open System.Collections.Generic
open System.Threading.Tasks
open Refit
open Blog.FSharp.Contracts.V1.Requests.CommentsRequests
open Blog.FSharp.Contracts.V1.Responses.CommentsResponses
open Blog.FSharp.Contracts.V1.Responses
open Blog.FSharp.Contracts.V1.Requests

/// <summary>
/// Comments controller requests api interface.
/// </summary>
type ICommentsControllerRequestsApi =

    /// <summary>
    /// Gets all comments.
    /// </summary>
    /// <returns>Task.</returns>
    /// <response code="200">Get all comments.</response>
    [<Get("/api/v1/comments")>]
    abstract member GetAllComments : unit -> Task<List<CommentResponse>>

    /// <summary>
    /// Gets the comments.
    /// </summary>
    /// <param name="sortParameters">The sort parameters.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Gets the comments.</response>
    /// <response code="404">Unable to gets the comments.</response>
    [<Get("/api/v1/comments/get-comments-by-filter")>]
    abstract member GetComments : ?sortParameters: SortParametersRequest -> Task<PagedCommentsResponse>

    /// <summary>
    /// Gets the comments by post asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="sortParameters">The sort parameters.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Gets the comments by post asynchronous.</response>
    /// <response code="404">Unable to gets the comments by post.</response>
    [<Get("/api/v1/comments/get-comments-by-post/{id}")>]
    abstract member GetCommentsByPostAsync : id:int * ?sortParameters: SortParametersRequest -> Task<List<CommentResponse>>

    /// <summary>
    /// Gets the comment.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Gets the comment.</response>
    /// <response code="404">Unable to gets the comment.</response>
    [<Get("/api/v1/comments/get-comment/{id}")>]
    abstract member GetComment : id:int -> Task<CommentResponse>

    /// <summary>
    /// Creates the asynchronous.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns>Task.</returns>
    /// <response code="201">Create the comment.</response>
    /// <response code="400">Unable to create the comment.</response>
    [<Headers("Authorization: Bearer")>]
    [<Post("/api/v1/comments/create")>]
    abstract member CreateAsync : request: CreateCommentRequest -> Task<CommentResponse>

    /// <summary>
    /// Edits the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="request">The request.</param>
    /// <returns>Task.</returns>
    /// <response code="204">Update the comment.</response>
    /// <response code="400">Unable to update the comment, because model is invalid.</response>
    /// <response code="404">Unable to update the comment, because comment not found.</response>
    [<Headers("Authorization: Bearer")>]
    [<Put("/api/v1/comments")>]
    abstract member EditAsync : id:int * request: UpdateCommentRequest -> Task<CommentResponse>

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Delete the comment.</response>
    /// <response code="404">Unable to delete the comment, because comment not found.</response>
    [<Headers("Authorization: Bearer")>]
    [<Delete("/api/v1/comments/{id}")>]
    abstract member DeleteAsync : id:int -> Task<CreatedResponse<int>>

