namespace Blog.FSharp.Contracts.V1.Requests.UsersRequests

open System.ComponentModel.DataAnnotations
open Blog.FSharp.Contracts.V1.Requests.Interfaces
open System.Collections.Generic

/// <summary>
/// Registration request.
/// </summary>
type RegistrationRequest() =
    interface IRequest

    /// <summary>
    /// Gets or sets email.
    /// </summary>
    [<Required>]
    [<EmailAddress>]
    member val Email : string = null with get, set

    /// <summary>
    /// Gets or sets password.
    /// </summary>
    [<Required>]
    [<MinLength(6)>]
    member val Password : string = null with get, set

    /// <summary>
    /// Gets or sets confirmPassword.
    /// </summary>
    [<Required>]
    member val ConfirmPassword : string = null with get, set

    /// <summary>
    /// Gets or sets firstName.
    /// </summary>
    [<Required>]
    [<StringLength(64, MinimumLength = 2, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")>]
    member val FirstName : string = null with get, set

    /// <summary>
    /// Gets or sets lastName.
    /// </summary>
    [<Required>]
    [<StringLength(64, MinimumLength = 2, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")>]
    member val LastName : string = null with get, set

    /// <summary>
    /// Gets or sets userName.
    /// </summary>
    member val UserName : string = null with get, set

    /// <summary>
    /// Gets or sets phoneNumber.
    /// </summary>
    member val PhoneNumber : string = null with get, set

    /// <summary>
    /// Gets or sets concurrencyStamp.
    /// </summary>
    member val ConcurrencyStamp : string = null with get, set

    /// <summary>
    /// Gets or sets roles.
    /// </summary>
    member val Roles : IEnumerable<string> = Seq.empty with get, set