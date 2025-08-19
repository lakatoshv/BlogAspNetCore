namespace Blog.FSharp.Contracts.V1.Requests.PostsRequests

open Blog.FSharp.Contracts.V1.Requests

/// <summary>
/// Posts search parameters request.
/// </summary>
/// <seealso cref="SearchParametersRequest" />
type PostsSearchParametersRequest() =
    inherit SearchParametersRequest()

    /// <summary>
    /// Gets or sets the tag.
    /// </summary>
    member val Tag : string = null with get, set