namespace Blog.Sdk.V1;

using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Contracts.V1.Requests;
using Blog.Contracts.V1.Requests.CommentsRequests;
using Blog.Contracts.V1.Responses;
using Blog.Contracts.V1.Responses.CommentsResponses;

/// <summary>
/// Comments controller requests api interface.
/// </summary>
public interface ICommentsControllerRequestsApi
{
    /// <summary>
    /// Gets all comments.
    /// </summary>
    /// <returns>Task.</returns>
    /// <response code="200">Get all comments.</response>
    [Get("/api/v1/comments")]
    Task<List<CommentResponse>> GetAllComments();

    /// <summary>
    /// Gets the comments.
    /// </summary>
    /// <param name="sortParameters">The sort parameters.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Gets the comments.</response>
    /// <response code="404">Unable to gets the comments.</response>
    [Get("/api/v1/comments/get-comments-by-filter")]
    Task<PagedCommentsResponse> GetComments(SortParametersRequest sortParameters = null);

    /// <summary>
    /// Gets the comments by post asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="sortParameters">The sort parameters.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Gets the comments by post asynchronous.</response>
    /// <response code="404">Unable to gets the comments by post.</response>
    [Get("/api/v1/comments/get-comments-by-post/{id}")]
    Task<List<CommentResponse>> GetCommentsByPostAsync(int id, SortParametersRequest sortParameters = null);

    /// <summary>
    /// Gets the comment.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Gets the comment.</response>
    /// <response code="404">Unable to gets the comment.</response>
    [Get("/api/v1/comments/get-comment/{id}")]
    Task<CommentResponse> GetComment(int id);

    /// <summary>
    /// Creates the asynchronous.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns>Task.</returns>
    /// <response code="201">Create the comment.</response>
    /// <response code="400">Unable to create the comment.</response>
    [Headers("Authorization: Bearer")]
    [Post("/api/v1/comments/create")]
    Task<CommentResponse> CreateAsync(CreateCommentRequest request);

    /// <summary>
    /// Edits the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="request">The request.</param>
    /// <returns>Task.</returns>
    /// <response code="204">Update the comment.</response>
    /// <response code="400">Unable to update the comment, because model is invalid.</response>
    /// <response code="404">Unable to update the comment, because comment not found.</response>
    [Headers("Authorization: Bearer")]
    [Put("/api/v1/comments")]
    Task<CommentResponse> EditAsync(int id, UpdateCommentRequest request);

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Delete the comment.</response>
    /// <response code="404">Unable to delete the comment, because comment not found.</response>
    [Headers("Authorization: Bearer")]
    [Delete("/api/v1/comments/{id}")]
    Task<CreatedResponse<int>> DeleteAsync(int id);
}