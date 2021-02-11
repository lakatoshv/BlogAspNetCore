// <copyright file="IWebHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Interfaces
{
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Web Helper interface.
    /// </summary>
    public interface IWebHelper
    {
        /// <summary>
        /// Gets a value indicating whether isRequestBeingRedirected.
        /// </summary>
        bool IsRequestBeingRedirected { get; }

        /// <summary>
        /// Gets or sets a value indicating whether isPostBeingDone.
        /// </summary>
        bool IsPostBeingDone { get; set; }

        /// <summary>
        /// Gets currentRequestProtocol.
        /// </summary>
        string CurrentRequestProtocol { get; }

        /// <summary>
        /// Get url referrer.
        /// </summary>
        /// <returns>string.</returns>
        string GetUrlReferrer();

        /// <summary>
        /// Get current ip address.
        /// </summary>
        /// <returns>string.</returns>
        string GetCurrentIpAddress();

        /// <summary>
        /// Get this page url.
        /// </summary>
        /// <param name="includeQueryString">includeQueryString.</param>
        /// <param name="useSsl">useSsl.</param>
        /// <param name="lowercaseUrl">lowercaseUrl.</param>
        /// <returns>string.</returns>
        string GetThisPageUrl(bool includeQueryString, bool? useSsl = null, bool lowercaseUrl = false);

        /// <summary>
        /// Is current connection secured.
        /// </summary>
        /// <returns>bool.</returns>
        bool IsCurrentConnectionSecured();

        /// <summary>
        /// Get store host.
        /// </summary>
        /// <param name="useSsl">useSsl.</param>
        /// <returns>string.</returns>
        string GetStoreHost(bool useSsl);

        /// <summary>
        /// Get store location.
        /// </summary>
        /// <param name="useSsl">useSsl.</param>
        /// <returns>string.</returns>
        string GetStoreLocation(bool? useSsl = null);

        /// <summary>
        /// Is static resource.
        /// </summary>
        /// <returns>bool.</returns>
        bool IsStaticResource();

        /// <summary>
        /// Modify query string.
        /// </summary>
        /// <param name="url">url.</param>
        /// <param name="key">key.</param>
        /// <param name="values">values.</param>
        /// <returns>string.</returns>
        string ModifyQueryString(string url, string key, params string[] values);

        /// <summary>
        /// Remove query string.
        /// </summary>
        /// <param name="url">url.</param>
        /// <param name="key">key.</param>
        /// <param name="value">value.</param>
        /// <returns>string.</returns>
        string RemoveQueryString(string url, string key, string value = null);

        /// <summary>
        /// Query string.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="name">name.</param>
        /// <returns>Type.</returns>
        T QueryString<T>(string name);

        /// <summary>
        /// Restart app domain.
        /// </summary>
        /// <param name="makeRedirect">makeRedirect.</param>
        void RestartAppDomain(bool makeRedirect = false);

        /// <summary>
        /// Is local request.
        /// </summary>
        /// <param name="req">req.</param>
        /// <returns>bool.</returns>
        bool IsLocalRequest(HttpRequest req);

        /// <summary>
        /// Get raw url.
        /// </summary>
        /// <param name="request">request.</param>
        /// <returns>string.</returns>
        string GetRawUrl(HttpRequest request);
    }
}
