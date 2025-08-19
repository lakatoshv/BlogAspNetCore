namespace Blog.FSharp.Contracts.V1.Requests.MessagesRequests

open System.ComponentModel.DataAnnotations
open Blog.FSharp.Contracts.V1.Requests.Interfaces

/// <summary>
/// Update message request.
/// </summary>
type UpdateMessageRequest() =
    interface IRequest

    /// <summary>
    /// Gets or sets the message identifier.
    /// </summary>
    [<Required>]
    member val MessageId : int = 0 with get, set

    /// <summary>
    /// Gets or sets the sender identifier.
    /// </summary>
    member val SenderId : string = null with get, set

    /// <summary>
    /// Gets or sets the recipient identifier.
    /// </summary>
    member val RecipientId : string = null with get, set

    /// <summary>
    /// Gets or sets the sender email.
    /// </summary>
    member val SenderEmail : string = null with get, set

    /// <summary>
    /// Gets or sets the name of the sender.
    /// </summary>
    member val SenderName : string = null with get, set

    /// <summary>
    /// Gets or sets the subject.
    /// </summary>
    member val Subject : string = null with get, set

    /// <summary>
    /// Gets or sets the body.
    /// </summary>
    member val Body : string = null with get, set

    /// <summary>
    /// Gets or sets the type of the message.
    /// </summary>
    member val MessageType : int = 0 with get, set