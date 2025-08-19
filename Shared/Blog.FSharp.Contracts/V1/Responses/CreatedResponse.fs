namespace Blog.FSharp.Contracts.V1.Responses

/// <summary>
/// Created response.
/// </summary>
/// <typeparam name="T"></typeparam>
type CreatedResponse<'T>() =
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    member val Id: 'T = Unchecked.defaultof<'T> with get, set