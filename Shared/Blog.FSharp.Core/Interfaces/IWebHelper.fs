namespace Blog.FSharp.Core.Interfaces

open Microsoft.AspNetCore.Http

/// <summary>
/// Web Helper interface.
/// </summary>
type IWebHelper =
    /// <summary>
    /// Gets a value indicating whether isRequestBeingRedirected.
    /// </summary>
    abstract member IsRequestBeingRedirected: bool with get

    /// <summary>
    /// Gets or sets a value indicating whether isPostBeingDone.
    /// </summary>
    abstract member IsPostBeingDone: bool with get, set

    /// <summary>
    /// Gets currentRequestProtocol.
    /// </summary>
    abstract member CurrentRequestProtocol: string with get

    /// <summary>
    /// Get url referrer.
    /// </summary>
    /// <returns>string.</returns>
    abstract member GetUrlReferrer: unit -> string

    /// <summary>
    /// Get current ip address.
    /// </summary>
    /// <returns>string.</returns>
    abstract member GetCurrentIpAddress: unit -> string

    /// <summary>
    /// Get this page url.
    /// </summary>
    /// <param name="includeQueryString">includeQueryString.</param>
    /// <param name="useSsl">useSsl.</param>
    /// <param name="lowercaseUrl">lowercaseUrl.</param>
    /// <returns>string.</returns>
    abstract member GetThisPageUrl: includeQueryString: bool * ?useSsl: bool * ?lowercaseUrl: bool -> string

    /// <summary>
    /// Is current connection secured.
    /// </summary>
    /// <returns>bool.</returns>
    abstract member IsCurrentConnectionSecured: unit -> bool

    /// <summary>
    /// Get store host.
    /// </summary>
    /// <param name="useSsl">useSsl.</param>
    /// <returns>string.</returns>
    abstract member GetStoreHost: useSsl: bool -> string

    /// <summary>
    /// Get store location.
    /// </summary>
    /// <param name="useSsl">useSsl.</param>
    /// <returns>string.</returns>
    abstract member GetStoreLocation: ?useSsl: bool -> string

    /// <summary>
    /// Is static resource.
    /// </summary>
    /// <returns>bool.</returns>
    abstract member IsStaticResource: unit -> bool

    /// <summary>
    /// Modify query string.
    /// </summary>
    /// <param name="url">url.</param>
    /// <param name="key">key.</param>
    /// <param name="values">values.</param>
    /// <returns>string.</returns>
    abstract member ModifyQueryString: url: string * key: string * values: string[] -> string

    /// <summary>
    /// Remove query string.
    /// </summary>
    /// <param name="url">url.</param>
    /// <param name="key">key.</param>
    /// <param name="value">value.</param>
    /// <returns>string.</returns>
    abstract member RemoveQueryString: url: string * key: string * ?value: string -> string

    /// <summary>
    /// Query string.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="name">name.</param>
    /// <returns>The T.</returns>
    abstract member QueryString<'T>: name: string -> 'T

    /// <summary>
    /// Restart app domain.
    /// </summary>
    /// <param name="makeRedirect">makeRedirect.</param>
    abstract member RestartAppDomain: ?makeRedirect: bool -> unit

    /// <summary>
    /// Is local request.
    /// </summary>
    /// <param name="req">req.</param>
    /// <returns>bool.</returns>
    abstract member IsLocalRequest: req: HttpRequest -> bool

    /// <summary>
    /// Get raw url.
    /// </summary>
    /// <param name="request">request.</param>
    /// <returns>string.</returns>
    abstract member GetRawUrl: request: HttpRequest -> string


