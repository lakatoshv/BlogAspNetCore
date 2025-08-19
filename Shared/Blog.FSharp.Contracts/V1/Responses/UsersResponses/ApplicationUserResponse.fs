namespace Blog.FSharp.Contracts.V1.Responses.UsersResponses

open System
open System.Collections.Generic
open Microsoft.AspNetCore.Identity

/// <summary>
/// Profile response.
/// </summary>
type ProfileResponse() =
    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    member val UserId: string = String.Empty with get, set

    /// <summary>
    /// Gets or sets the user.
    /// </summary>
    member val User: ApplicationUserResponse option = None with get, set

    /// <summary>
    /// Gets or sets the About.
    /// </summary>
    member val About: string = String.Empty with get, set

    /// <summary>
    /// Gets or sets the profile img.
    /// </summary>
    member val ProfileImg: string = String.Empty with get, set

/// <summary>
/// Application user response.
/// </summary>
and ApplicationUserResponse() =
    /// <summary>
    /// Gets or sets first name.
    /// </summary>
    member val FirstName: string = String.Empty with get, set

    /// <summary>
    /// Gets or sets last name.
    /// </summary>
    member val LastName: string = String.Empty with get, set

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    member val Email: string = String.Empty with get, set

    /// <summary>
    /// Gets or sets a value indicating whether email confirmed.
    /// </summary>
    member val EmailConfirmed: bool = false with get, set

    /// <summary>
    /// Gets or sets roles.
    /// </summary>
    member val Roles: ICollection<IdentityUserRole<string>> = List<IdentityUserRole<string>>() :> ICollection<_> with get, set

    /// <summary>
    /// Gets or sets the phone number.
    /// </summary>
    member val PhoneNumber: string = String.Empty with get, set

    /// <summary>
    /// Gets or sets a value indicating whether phone number confirmed.
    /// </summary>
    member val PhoneNumberConfirmed: bool = false with get, set

    /// <summary>
    /// Gets or sets the profile.
    /// </summary>
    member val Profile: ProfileResponse option = None with get, set