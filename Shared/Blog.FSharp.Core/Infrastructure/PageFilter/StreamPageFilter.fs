namespace Blog.FSharp.Core.Infrastructure.PageFilter

/// <summary>
/// Stream page filter.
/// </summary>
type StreamPageFilter() =
    /// <summary>
    /// Gets or sets previous value.
    /// </summary>
    member val PreviousValue: int = 0 with get, set

    /// <summary>
    /// Gets or sets length.
    /// </summary>
    member val Length: int = 0 with get, set

    /// <summary>
    /// Gets or sets page count.
    /// </summary>
    member val PageCount: int = 0 with get, set
