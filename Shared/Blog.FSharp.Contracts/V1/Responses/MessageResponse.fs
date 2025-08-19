namespace Blog.FSharp.Contracts.V1.Responses

open Blog.FSharp.Core.Enums
open Blog.FSharp.Contracts.V1.Responses.UsersResponses

/// <summary>
/// Message response.
/// </summary>
type MessageResponse() =

    /// <summary>
    /// Gets or sets the sender identifier.
    /// </summary>
    member val SenderId: string = "" with get, set

    /// <summary>
    /// Gets or sets the sender.
    /// </summary>
    member val Sender: ApplicationUserResponse|null = null with get, set

    /// <summary>
    /// Gets or sets the recipient identifier.
    /// </summary>
    member val RecipientId: string = "" with get, set

    /// <summary>
    /// Gets or sets the recipient.
    /// </summary>
    member val Recipient: ApplicationUserResponse|null = null with get, set

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
    member val MessageType: MessageType = Message with get, set