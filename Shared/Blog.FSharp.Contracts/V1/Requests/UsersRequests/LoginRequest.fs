namespace Blog.FSharp.Contracts.V1.Requests.UsersRequests

open System.ComponentModel.DataAnnotations
open Blog.FSharp.Contracts.V1.Requests.Interfaces

/// <summary>
/// Login request.
/// </summary>
type LoginRequest() =
    interface IRequest

    /// <summary>
    /// Gets or sets email.
    /// </summary>
    [<Required>]
    [<StringLength(64, MinimumLength = 2, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")>]
    member val Email : string = null with get, set

    /// <summary>
    /// Gets or sets password.
    /// </summary>
    [<Required>]
    [<StringLength(64, MinimumLength = 2, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")>]
    member val Password : string = null with get, set

