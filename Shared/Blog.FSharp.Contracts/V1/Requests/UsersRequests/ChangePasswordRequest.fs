namespace Blog.FSharp.Contracts.V1.Requests.UsersRequests

open System.ComponentModel.DataAnnotations
open Blog.FSharp.Contracts.V1.Requests.Interfaces

/// <summary>
/// Change password request.
/// </summary>
type ChangePasswordRequest() =
    interface IRequest

    /// <summary>
    /// Gets or sets oldPassword.
    /// </summary>
    [<Required>]
    [<DataType(DataType.Password)>]
    [<Display(Name = "Current password")>]
    member val OldPassword : string = null with get, set

    /// <summary>
    /// Gets or sets newPassword.
    /// </summary>
    [<Required>]
    [<StringLength(100, MinimumLength = 6, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")>]
    [<DataType(DataType.Password)>]
    [<Display(Name = "New password")>]
    member val NewPassword : string = null with get, set