namespace Blog.FSharp.Core.Emails

open System.Collections.Generic

/// <summary>
/// Email.
/// </summary>
type Email() =

    /// <summary>
    /// Gets or sets body.
    /// </summary>
    member val Body : string = "" with get, set

    /// <summary>
    /// Gets or sets subject.
    /// </summary>
    member val Subject : string = "" with get, set

    /// <summary>
    /// Gets or sets from.
    /// </summary>
    member val From : string = "" with get, set

    /// <summary>
    /// Gets or sets to.
    /// </summary>
    member val To : string = "" with get, set

    /// <summary>
    /// Gets or sets attachments.
    /// </summary>
    member val Attachments : List<byte[]> = List<byte[]>() with get, set

