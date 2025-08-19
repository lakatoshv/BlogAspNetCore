namespace Blog.FSharp.Contracts.V1.Requests.UsersRequests

/// <summary>
/// Update profile request.
/// </summary>
type UpdateProfileRequest() =
    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    member val Email: string = "" with get, set

    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    member val FirstName: string = "" with get, set

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    member val LastName: string = "" with get, set

    /// <summary>
    /// Gets or sets the phone number.
    /// </summary>
    member val PhoneNumber: string = "" with get, set

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    member val Password: string = "" with get, set

    /// <summary>
    /// Gets or sets the About.
    /// </summary>
    member val About: string = "" with get, set