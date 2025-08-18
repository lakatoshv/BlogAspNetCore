namespace Blog.FSharp.Core

/// <summary>
/// Claims identity user model.
/// </summary>
type ClaimsIdentityUserModel =
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        Id: string

        /// <summary>
        /// Gets or sets userName.
        /// </summary>
        UserName: string

        /// <summary>
        /// Gets or sets email.
        /// </summary>
        Email: string

        /// <summary>
        /// Gets or sets phone number.
        /// </summary>
        PhoneNumber: string

        /// <summary>
        /// Gets or sets a value indicating whether is email verified.
        /// </summary>
        IsEmailVerified: bool

        /// <summary>
        /// Gets or sets the profile identifier.
        /// </summary>
        ProfileId: int
    }