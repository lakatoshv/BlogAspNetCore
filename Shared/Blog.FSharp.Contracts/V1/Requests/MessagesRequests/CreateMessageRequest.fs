namespace Blog.FSharp.Contracts.V1.Requests.MessagesRequests

open Blog.FSharp.Contracts.V1.Requests.Interfaces

/// <summary>
/// Create message request.
/// </summary>
type CreateMessageRequest() =
    interface IRequest

    /// <summary>
    /// Gets or sets the sender identifier.
    /// </summary>
    member val SenderId: string = "" with get, set

    /// <summary>
    /// Gets or sets the recipient identifier.
    /// </summary>
    member val RecipientId: string = "" with get, set

    /// <summary>
    /// Gets or sets the sender email.
    /// </summary>
    member val SenderEmail: string = "" with get, set

    /// <summary>
    /// Gets or sets the name of the sender.
    /// </summary>
    member val SenderName: string = "" with get, set

    /// <summary>
    /// Gets or sets the subject.
    /// </summary>
    member val Subject: string = "" with get, set

    /// <summary>
    /// Gets or sets the body.
    /// </summary>
    member val Body: string = "" with get, set

    /// <summary>
    /// Gets or sets the type of the message.
    /// </summary>
    member val MessageType: int = 0 with get, set