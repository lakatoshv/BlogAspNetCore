using Blog.Contracts.V1;
using Blog.Contracts.V1.Requests;
using Blog.Contracts.V1.Requests.CommentsRequests;
using Blog.Contracts.V1.Responses;
using Blog.Contracts.V1.Responses.CommentsResponses;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Sdk.V1
{
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
        [Get(ApiRoutes.CommentsController.Comments)]
        Task<ApiResponse<List<CommentResponse>>> GetAllComments();

        /// <summary>
        /// Gets the comments.
        /// </summary>
        /// <param name="sortParameters">The sort parameters.</param>
        /// <returns>Task.</returns>
        /// <response code="200">Gets the comments.</response>
        /// <response code="404">Unable to gets the comments.</response>
        [Get(ApiRoutes.CommentsController.Comments + "/" + ApiRoutes.CommentsController.GetCommentsByFilter)]
        Task<ApiResponse<PagedCommentsResponse>> GetComments([FromBody] SortParametersRequest sortParameters = null);

        /// <summary>
        /// Gets the comments by post asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="sortParameters">The sort parameters.</param>
        /// <returns>Task.</returns>
        /// <response code="200">Gets the comments by post asynchronous.</response>
        /// <response code="404">Unable to gets the comments by post.</response>
        [Get(ApiRoutes.CommentsController.Comments + "/" + ApiRoutes.CommentsController.GetCommentsByPost)]
        Task<ApiResponse<List<CommentResponse>>> GetCommentsByPostAsync([FromRoute] int id, [FromBody] SortParametersRequest sortParameters = null);

        /// <summary>
        /// Gets the comment.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        /// <response code="200">Gets the comment.</response>
        /// <response code="404">Unable to gets the comment.</response>
        [Get(ApiRoutes.CommentsController.Comments + "/" + ApiRoutes.CommentsController.GetComment)]
        Task<ApiResponse<CommentResponse>> GetComment([FromRoute] int id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task.</returns>
        /// <response code="201">Create the comment.</response>
        /// <response code="400">Unable to create the comment.</response>
        [Headers("Authorization: Bearer")]
        [Post(ApiRoutes.CommentsController.Comments + "/" + ApiRoutes.CommentsController.CreateComment)]
        Task<ApiResponse<CommentResponse>> CreateAsync([FromBody] CreateCommentRequest request);

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
        [Put(ApiRoutes.CommentsController.Comments + "/" + ApiRoutes.CommentsController.EditComment)]
        Task<ApiResponse<CommentResponse>> EditAsync([FromRoute] int id, [FromBody] UpdateCommentRequest request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        /// <response code="200">Delete the comment.</response>
        /// <response code="404">Unable to delete the comment, because comment not found.</response>
        [Headers("Authorization: Bearer")]
        [Delete(ApiRoutes.CommentsController.Comments + "/" + ApiRoutes.CommentsController.EditComment)]
        Task<ApiResponse<CreatedResponse<int>>> DeleteAsync([FromRoute] int id);
    }
}
