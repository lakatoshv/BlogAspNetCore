namespace Blog.FSharp.Core.Constants

/// <summary>
/// Constants.
/// </summary>
module Constants =

    /// <summary>
    /// Policy Prefix.
    /// </summary>
    [<Literal>] 
    let PolicyPrefix = "Permission"

    /// <summary>
    /// Month Count.
    /// </summary>
    [<Literal>] 
    let MonthCount = 12

    /// <summary>
    /// First Index.
    /// </summary>
    [<Literal>] 
    let FirstIndex = 1

    /// <summary>
    /// Default Cache Time Minutes.
    /// </summary>
    [<Literal>] 
    let DefaultCacheTimeMinutes = 60

    /// <summary>
    /// The application name.
    /// </summary>
    [<Literal>] 
    let ApplicationName = "Blog Web Api"

    /// <summary>
    /// The JSON type.
    /// </summary>
    [<Literal>] 
    let JsonType = "application/json"

    /// <summary>
    /// Roles.
    /// </summary>
    module Roles =

        /// <summary>
        /// The user.
        /// </summary>
        [<Literal>] 
        let User = "User"