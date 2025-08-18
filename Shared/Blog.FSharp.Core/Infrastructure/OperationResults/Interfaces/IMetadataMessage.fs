namespace Blog.FSharp.Core.Infrastructure.OperationResults.Interfaces

/// <summary>
/// Metadata message interface.
/// </summary>
/// <seealso cref="IHaveDataObject" />
type IMetadataMessage =
    inherit IHaveDataObject

    /// <summary>
    /// Gets the message.
    /// </summary>
    /// <value>
    /// The message.
    /// </value>
    abstract member Message : string

    /// <summary>
    /// Gets the data object.
    /// </summary>
    /// <value>
    /// The data object.
    /// </value>
    abstract member DataObject : obj

