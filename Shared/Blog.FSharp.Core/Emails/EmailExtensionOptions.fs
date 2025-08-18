namespace Blog.FSharp.Core.Emails

namespace Blog.Core.Options

/// <summary>
/// Email Extension Options.
/// </summary>
type EmailExtensionOptions() =

    /// <summary>
    /// Gets or sets baseUrl.
    /// </summary>
    member val BaseUrl : string = "" with get, set

    /// <summary>
    /// Gets or sets from.
    /// </summary>
    member val From : string = "" with get, set