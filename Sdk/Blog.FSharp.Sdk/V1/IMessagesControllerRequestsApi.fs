namespace Blog.FSharp.Sdk.V1

open System.Collections.Generic
open System.Threading.Tasks
open Refit
open Blog.FSharp.Contracts.V1.Responses
open Blog.FSharp.Contracts.V1.Requests.MessagesRequests

/// <summary>
/// Messages controller requests api interface.
/// </summary>
type IMessagesControllerRequestsApi =

    /// <summary>
    /// Indexes this instance.
    /// </summary>
    /// <returns>Task.</returns>
    /// <response code="200">Get all messages.</response>
    /// <response code="404">Unable to get all messages.</response>
    [<Get("/api/v1/messages")>]
    abstract member Index : unit -> Task<List<MessageResponse>>

    /// <summary>
    /// Gets the recipient messages.
    /// </summary>
    /// <param name="recipientId">The recipient identifier.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Gets the recipient messages.</response>
    /// <response code="404">Unable to gets the recipient messages.</response>
    [<Get("/api/v1/messages/get-recipient-messages/{recipientId}")>]
    abstract member GetRecipientMessages : recipientId:string -> Task<List<MessageResponse>>

    /// <summary>
    /// Gets the sender messages.
    /// </summary>
    /// <param name="senderEmail">The sender email.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Gets the sender messages.</response>
    /// <response code="404">Unable to gets the sender messages.</response>
    [<Get("/api/v1/messages/get-sender-messages/{senderEmail}")>]
    abstract member GetSenderMessages : senderEmail:string -> Task<List<MessageResponse>>

    /// <summary>
    /// Shows the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task</returns>
    /// <response code="200">Gets the messages by id.</response>
    /// <response code="404">Unable to gets the messages by id.</response>
    [<Get("/api/v1/messages/show/{id}")>]
    abstract member Show : id:int -> Task<ApiResponse<MessageResponse>>

    /// <summary>
    /// Creates the asynchronous.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns>Task</returns>
    /// <response code="201">Creates the message.</response>
    /// <response code="400">Unable to create the message.</response>
    [<Post("/api/v1/messages")>]
    abstract member CreateAsync : request:CreateMessageRequest -> Task<MessageResponse>

    /// <summary>
    /// Edits the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="request">The request.</param>
    /// <returns>Task.</returns>
    /// <response code="204">Edit the message.</response>
    /// <response code="400">Unable to edit the message, model is invalid.</response>
    /// <response code="404">Unable to edit the message, message not found.</response>
    [<Put("/api/v1/message/{id}")>]
    abstract member EditAsync : id:int * request:UpdateMessageRequest -> Task<MessageResponse>

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="authorId">The author identifier.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Delete the message.</response>
    /// <response code="404">Unable to delete the message, message not found.</response>
    [<Delete("/api/v1/message/{id}")>]
    abstract member DeleteAsync : id:int * authorId:string -> Task<CreatedResponse<int>>

