namespace Blog.FSharp.Core.Infrastructure.OperationResults

/// <summary>
/// Metadata type.
/// Defines the category or severity of metadata messages.
/// </summary>
[<RequireQualifiedAccess>]
type MetadataType =
    /// <summary>
    /// Informational message.
    /// Used for logging or providing additional context.
    /// </summary>
    | Info = 0

    /// <summary>
    /// Success message.
    /// Indicates that an operation completed successfully.
    /// </summary>
    | Success = 1

    /// <summary>
    /// Warning message.
    /// Indicates a potential issue that does not prevent execution.
    /// </summary>
    | Warning = 2

    /// <summary>
    /// Error message.
    /// Indicates that an operation failed or encountered a critical issue.
    /// </summary>
    | Error = 3

