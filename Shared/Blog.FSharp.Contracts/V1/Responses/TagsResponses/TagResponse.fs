namespace Blog.FSharp.Contracts.V1.Responses.TagsResponses

/// <summary>
/// Tag response.
/// </summary>
type TagResponse() =
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    member val Id: int = 0 with get, set

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    member val Title: string = "" with get, set