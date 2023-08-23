namespace Blog.Sdk.V1;

using Blog.Contracts.V1.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using Blog.Contracts.V1.Requests.MessagesRequests;

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
    [Get("/api/v1/messages")]
    Task<List<MessageResponse>> Index();

    /// <summary>
    /// Gets the recipient messages.
    /// </summary>
    /// <param name="recipientId">The recipient identifier.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Gets the recipient messages.</response>
    /// <response code="404">Unable to gets the recipient messages.</response>
    [Get("/api/v1/messages/get-recipient-messages/{recipientId}")]
    Task<List<MessageResponse>> GetRecipientMessages(string recipientId);

    /// <summary>
    /// Gets the sender messages.
    /// </summary>
    /// <param name="senderEmail">The sender email.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Gets the sender messages.</response>
    /// <response code="404">Unable to gets the sender messages.</response>
    [Get("/api/v1/messages/get-sender-messages/{senderEmail}")]
    Task<List<MessageResponse>> GetSenderMessages(string senderEmail);

    /// <summary>
    /// Shows the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task</returns>
    /// <response code="200">Gets the messages by id.</response>
    /// <response code="404">Unable to gets the messages by id.</response>
    [Get("/api/v1/messages/show/{id}")]
    Task<ApiResponse<MessageResponse>> Show(int id);

    /// <summary>
    /// Creates the asynchronous.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns>Task</returns>
    /// <response code="201">Creates the message.</response>
    /// <response code="400">Unable to create the message.</response>
    [Post("/api/v1/messages")]
    Task<MessageResponse> CreateAsync(CreateMessageRequest request);

    /// <summary>
    /// Edits the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="request">The request.</param>
    /// <returns>Task.</returns>
    /// <response code="204">Edit the message.</response>
    /// <response code="400">Unable to edit the message, model is invalid.</response>
    /// <response code="404">Unable to edit the message, message not found.</response>
    [Put("/api/v1/message/{id}")]
    Task<MessageResponse> EditAsync(int id, UpdateMessageRequest request);

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="authorId">The author identifier.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Delete the message.</response>
    /// <response code="404">Unable to delete the message, message not found.</response>
    [Delete("/api/v1/message/{id}")]
    Task<CreatedResponse<int>> DeleteAsync(int id, string authorId);
}