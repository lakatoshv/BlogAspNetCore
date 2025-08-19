namespace Blog.FSharp.Contracts

/// <summary>
/// Error model.
/// </summary>
type ErrorModel() =
    /// <summary>
    /// Gets or sets the name of the field.
    /// </summary>
    member val FieldName: string = "" with get, set

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    member val Message: string = "" with get, set