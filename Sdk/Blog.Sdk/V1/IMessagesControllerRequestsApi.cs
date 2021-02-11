using Blog.Contracts.V1.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Requests.MessagesRequests;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace Blog.Sdk.V1
{
    /// <summary>
    /// Messages controller requests api interface.
    /// </summary>
    public interface IMessagesControllerRequestsApi
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>Task.</returns>
        /// <response code="200">Get all messages.</response>
        /// <response code="404">Unable to get all messages.</response>
        [Get(ApiRoutes.MessagesController.Messages)]
        Task<ApiResponse<List<MessageResponse>>> Index();

        /// <summary>
        /// Gets the recipient messages.
        /// </summary>
        /// <param name="recipientId">The recipient identifier.</param>
        /// <returns>Task.</returns>
        /// <response code="200">Gets the recipient messages.</response>
        /// <response code="404">Unable to gets the recipient messages.</response>
        [Get(ApiRoutes.MessagesController.Messages  + "/" + ApiRoutes.MessagesController.GetRecipientMessages)]
        Task<ApiResponse<List<MessageResponse>>> GetRecipientMessages([FromRoute] string recipientId);

        /// <summary>
        /// Gets the sender messages.
        /// </summary>
        /// <param name="senderEmail">The sender email.</param>
        /// <returns>Task.</returns>
        /// <response code="200">Gets the sender messages.</response>
        /// <response code="404">Unable to gets the sender messages.</response>
        [Get(ApiRoutes.MessagesController.Messages + "/" + ApiRoutes.MessagesController.GetSenderMessages)]
        Task<ApiResponse<List<MessageResponse>>> GetSenderMessages([FromRoute] string senderEmail);

        /// <summary>
        /// Shows the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task</returns>
        /// <response code="200">Gets the messages by id.</response>
        /// <response code="404">Unable to gets the messages by id.</response>
        [Get(ApiRoutes.MessagesController.Messages + "/show")]
        Task<ApiResponse<MessageResponse>> Show([FromRoute] int id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task</returns>
        /// <response code="201">Creates the message.</response>
        /// <response code="400">Unable to create the message.</response>
        [Post(ApiRoutes.MessagesController.Messages)]
        Task<ApiResponse<MessageResponse>> CreateAsync([FromBody] CreateMessageRequest request);

        /// <summary>
        /// Edits the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>Task.</returns>
        /// <response code="204">Edit the message.</response>
        /// <response code="400">Unable to edit the message, model is invalid.</response>
        /// <response code="404">Unable to edit the message, message not found.</response>
        [Put(ApiRoutes.MessagesController.Messages)]
        Task<ApiResponse<MessageResponse>> EditAsync([FromRoute] int id, [FromBody] UpdateMessageRequest request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="authorId">The author identifier.</param>
        /// <returns>Task.</returns>
        /// <response code="200">Delete the message.</response>
        /// <response code="404">Unable to delete the message, message not found.</response>
        [Delete(ApiRoutes.MessagesController.Messages)]
        Task<ApiResponse<CreatedResponse<int>>> DeleteAsync([FromRoute] int id, [FromBody] string authorId);
    }
}