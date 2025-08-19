namespace Blog.FSharp.Contracts.V1.Responses.UsersResponses

open System
open System.Collections.Generic

/// <summary>
/// Account response.
/// </summary>
type AccountResponse() =
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    member val Id: Guid = Guid.Empty with get, set

    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    member val FirstName: string = String.Empty with get, set

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    member val LastName: string = String.Empty with get, set

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    member val UserName: string = String.Empty with get, set

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    member val Email: string = String.Empty with get, set

    /// <summary>
    /// Gets or sets the phone number.
    /// </summary>
    member val PhoneNumber: string = String.Empty with get, set

    /// <summary>
    /// Gets or sets the roles.
    /// </summary>
    member val Roles: IEnumerable<RoleResponse> = Seq.empty :> IEnumerable<RoleResponse> with get, set

