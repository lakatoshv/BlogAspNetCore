namespace Blog.FSharp.Core.Infrastructure

open System
open System.Runtime.Serialization

/// <summary>
/// Blog exception.
/// </summary>
[<Serializable>]
type BlogException =
    inherit Exception

    /// <summary>
    /// Initializes a new instance of the <see cref="BlogException"/> class.
    /// </summary>
    new () = { inherit Exception() }

    /// <summary>
    /// Initializes a new instance of the <see cref="BlogException"/> class.
    /// </summary>
    /// <param name="message">message.</param>
    new (message: string) = { inherit Exception(message) }

    /// <summary>
    /// Initializes a new instance of the <see cref="BlogException"/> class.
    /// </summary>
    /// <param name="messageFormat">messageFormat.</param>
    /// <param name="args">args.</param>
    new (messageFormat: string, [<ParamArray>] args: obj[]) =
        { inherit Exception(String.Format(messageFormat, args)) }

    /// <summary>
    /// Initializes a new instance of the <see cref="BlogException"/> class.
    /// </summary>
    /// <param name="message">message.</param>
    /// <param name="innerException">innerException.</param>
    new (message: string, innerException: Exception) =
        { inherit Exception(message, innerException) }

    /// <summary>
    /// Initializes a new instance of the <see cref="BlogException"/> class.
    /// </summary>
    /// <param name="info">info.</param>
    /// <param name="context">context.</param>
    new (info: SerializationInfo, context: StreamingContext) =
        { inherit Exception(info, context) }

