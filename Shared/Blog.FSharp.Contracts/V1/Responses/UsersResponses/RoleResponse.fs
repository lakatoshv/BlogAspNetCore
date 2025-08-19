namespace Blog.FSharp.Contracts.V1.Responses.UsersResponses

/// <summary>
/// Role response.
/// </summary>
type RoleResponse() =
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    member val Id: string = "" with get, set

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    member val Name: string = "" with get, set

