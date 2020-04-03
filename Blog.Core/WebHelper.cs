// <copyright file="WebHelper.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Configuration;
    using Helpers;
    using Infrastructure;
    using Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Primitives;
    using Microsoft.Net.Http.Headers;

    /// <summary>
    /// Web helper.
    /// </summary>
    public class WebHelper : IWebHelper
    {
        /// <summary>
        /// Null ip address.
        /// </summary>
        private const string NullIpAddress = "::1";

        /// <summary>
        /// Http context accessor.
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Hosting config.
        /// </summary>
        private readonly HostingConfig _hostingConfig;

        /// <summary>
        /// _File provider.
        /// </summary>
        private readonly IShareFileProvider _fileProvider;

        /// <inheritdoc/>
        public virtual bool IsRequestBeingRedirected
        {
            get
            {
                var response = this._httpContextAccessor.HttpContext.Response;

                // ASP.NET 4 style - return response.IsRequestBeingRedirected;
                int[] redirectionStatusCodes = { StatusCodes.Status301MovedPermanently, StatusCodes.Status302Found };
                return redirectionStatusCodes.Contains(response.StatusCode);
            }
        }

        /// <inheritdoc/>
        public virtual bool IsPostBeingDone
        {
            get
            {
                if (this._httpContextAccessor.HttpContext.Items["nop.IsPOSTBeingDone"] == null)
                {
                    return false;
                }

                return Convert.ToBoolean(this._httpContextAccessor.HttpContext.Items["nop.IsPOSTBeingDone"]);
            }

            set => this._httpContextAccessor.HttpContext.Items["nop.IsPOSTBeingDone"] = value;
        }

        /// <inheritdoc/>
        public virtual string CurrentRequestProtocol => this.IsCurrentConnectionSecured() ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebHelper"/> class.
        /// </summary>
        /// <param name="hostingConfig">hostingConfig.</param>
        /// <param name="httpContextAccessor">httpContextAccessor.</param>
        /// <param name="fileProvider">fileProvider.</param>
        public WebHelper(
            HostingConfig hostingConfig,
            IHttpContextAccessor httpContextAccessor,
            IShareFileProvider fileProvider)
        {
            this._hostingConfig = hostingConfig;
            this._httpContextAccessor = httpContextAccessor;
            this._fileProvider = fileProvider;
        }

        /// <inheritdoc/>
        public virtual string GetUrlReferrer()
        {
            if (!this.IsRequestAvailable())
            {
                return string.Empty;
            }

            // URL referrer is null in some case (for example, in IE 8)
            return this._httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Referer];
        }

        /// <inheritdoc/>
        public virtual string GetCurrentIpAddress()
        {
            if (!this.IsRequestAvailable())
            {
                return string.Empty;
            }

            var result = string.Empty;
            try
            {
                // first try to get IP address from the forwarded header
                if (this._httpContextAccessor.HttpContext.Request.Headers != null)
                {
                    // the X-Forwarded-For (XFF) HTTP header field is a de facto standard for identifying the originating IP address of a client
                    // connecting to a web server through an HTTP proxy or load balancer
                    var forwardedHttpHeaderKey = "X-FORWARDED-FOR";
                    if (!string.IsNullOrEmpty(this._hostingConfig.ForwardedHttpHeader))
                    {
                        // but in some cases server use other HTTP header
                        // in these cases an administrator can specify a custom Forwarded HTTP header (e.g. CF-Connecting-IP, X-FORWARDED-PROTO, etc)
                        forwardedHttpHeaderKey = this._hostingConfig.ForwardedHttpHeader;
                    }

                    var forwardedHeader = this._httpContextAccessor.HttpContext.Request.Headers[forwardedHttpHeaderKey];
                    if (!StringValues.IsNullOrEmpty(forwardedHeader))
                    {
                        result = forwardedHeader.FirstOrDefault();
                    }
                }

                // if this header not exists try get connection remote IP address
                if (string.IsNullOrEmpty(result) && this._httpContextAccessor.HttpContext.Connection.RemoteIpAddress != null)
                {
                    result = this._httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                }
            }
            catch
            {
                return string.Empty;
            }

            // some of the validation
            if (result != null && result.Equals("::1", StringComparison.InvariantCultureIgnoreCase))
            {
                result = "127.0.0.1";
            }

            // "TryParse" doesn't support IPv4 with port number
            if (IPAddress.TryParse(result ?? string.Empty, out var ip))
            {
                // IP address is valid
                result = ip.ToString();
            }
            else if (!string.IsNullOrEmpty(result))
            {
                // remove port
                result = result.Split(':').FirstOrDefault();
            }

            return result;
        }

        /// <inheritdoc/>
        public virtual string GetThisPageUrl(bool includeQueryString, bool? useSsl = null, bool lowercaseUrl = false)
        {
            if (!this.IsRequestAvailable())
            {
                return string.Empty;
            }

            // get store location
            var storeLocation = this.GetStoreLocation(useSsl ?? this.IsCurrentConnectionSecured());

            // add local path to the URL
            var pageUrl = $"{storeLocation.TrimEnd('/')}{this._httpContextAccessor.HttpContext.Request.Path}";

            // add query string to the URL
            if (includeQueryString)
            {
                pageUrl = $"{pageUrl}{this._httpContextAccessor.HttpContext.Request.QueryString}";
            }

            // whether to convert the URL to lower case
            if (lowercaseUrl)
            {
                pageUrl = pageUrl.ToLowerInvariant();
            }

            return pageUrl;
        }

        /// <inheritdoc/>
        public virtual bool IsCurrentConnectionSecured()
        {
            if (!this.IsRequestAvailable())
            {
                return false;
            }

            // check whether hosting uses a load balancer
            // use HTTP_CLUSTER_HTTPS?
            if (this._hostingConfig.UseHttpClusterHttps)
            {
                return this._httpContextAccessor.HttpContext.Request.Headers["HTTP_CLUSTER_HTTPS"].ToString().Equals("on", StringComparison.OrdinalIgnoreCase);
            }

            // use HTTP_X_FORWARDED_PROTO?
            if (this._hostingConfig.UseHttpXForwardedProto)
            {
                return this._httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-Proto"].ToString().Equals("https", StringComparison.OrdinalIgnoreCase);
            }

            return this._httpContextAccessor.HttpContext.Request.IsHttps;
        }

        /// <inheritdoc/>
        public virtual string GetStoreHost(bool useSsl)
        {
            if (!this.IsRequestAvailable())
            {
                return string.Empty;
            }

            // try to get host from the request HOST header
            var hostHeader = this._httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Host];
            if (StringValues.IsNullOrEmpty(hostHeader))
            {
                return string.Empty;
            }

            // add scheme to the URL
            var storeHost = $"{(useSsl ? Uri.UriSchemeHttps : Uri.UriSchemeHttp)}://{hostHeader.FirstOrDefault()}";

            // ensure that host is ended with slash
            storeHost = $"{storeHost.TrimEnd('/')}/";

            return storeHost;
        }

        /// <inheritdoc/>
        public virtual string GetStoreLocation(bool? useSsl = null)
        {
            var storeLocation = string.Empty;

            // get store host
            // var storeHost = GetStoreHost(useSsl ?? IsCurrentConnectionSecured());

            // ensure that URL is ended with slash
            storeLocation = $"{storeLocation.TrimEnd('/')}/";

            return storeLocation;
        }

        /// <inheritdoc/>
        public virtual bool IsStaticResource()
        {
            if (!this.IsRequestAvailable())
            {
                return false;
            }

            string path = this._httpContextAccessor.HttpContext.Request.Path;

            // a little workaround. FileExtensionContentTypeProvider contains most of static file extensions. So we can use it
            // source: https://github.com/aspnet/StaticFiles/blob/dev/src/Microsoft.AspNetCore.StaticFiles/FileExtensionContentTypeProvider.cs
            // if it can return content type, then it's a static file
            var contentTypeProvider = new FileExtensionContentTypeProvider();
            return contentTypeProvider.TryGetContentType(path, out string _);
        }

        /// <inheritdoc/>
        public virtual string ModifyQueryString(string url, string key, params string[] values)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(key))
            {
                return url;
            }

            // get current query parameters
            var uri = new Uri(url);
            var queryParameters = QueryHelpers.ParseQuery(uri.Query);

            // and add passed one
            queryParameters[key] = new StringValues(values);
            var queryBuilder = new QueryBuilder(queryParameters
                .ToDictionary(parameter => parameter.Key, parameter => parameter.Value.ToString()));

            // create new URL with passed query parameters
            url = $"{uri.GetLeftPart(UriPartial.Path)}{queryBuilder.ToQueryString()}{uri.Fragment}";

            return url;
        }

        /// <inheritdoc/>
        public virtual string RemoveQueryString(string url, string key, string value = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(key))
            {
                return url;
            }

            // get current query parameters
            var uri = new Uri(url);
            var queryParameters = QueryHelpers.ParseQuery(uri.Query)
                .SelectMany(parameter => parameter.Value, (parameter, queryValue) => new KeyValuePair<string, string>(parameter.Key, queryValue))
                .ToList();

            if (!string.IsNullOrEmpty(value))
            {
                // remove a specific query parameter value if it's passed
                queryParameters.RemoveAll(parameter => parameter.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)
                    && parameter.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                // or remove query parameter by the key
                queryParameters.RemoveAll(parameter => parameter.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));
            }

            // create new URL without passed query parameters
            url = $"{uri.GetLeftPart(UriPartial.Path)}{new QueryBuilder(queryParameters).ToQueryString()}{uri.Fragment}";

            return url;
        }

        /// <inheritdoc/>
        public virtual T QueryString<T>(string name)
        {
            if (!this.IsRequestAvailable())
            {
                return default(T);
            }

            if (StringValues.IsNullOrEmpty(this._httpContextAccessor.HttpContext.Request.Query[name]))
            {
                return default(T);
            }

            return CommonHelper.To<T>(this._httpContextAccessor.HttpContext.Request.Query[name].ToString());
        }

        /// <inheritdoc/>
        public virtual void RestartAppDomain(bool makeRedirect = false)
        {
            // the site will be restarted during the next request automatically
            // _applicationLifetime.StopApplication();

            // "touch" web.config to force restart
            var success = this.TryWriteWebConfig();
            if (!success)
            {
                throw new BlogException("Something went wrong");
            }
        }

        /// <inheritdoc/>
        public virtual bool IsLocalRequest(HttpRequest req)
        {
            // source: https://stackoverflow.com/a/41242493/7860424
            var connection = req.HttpContext.Connection;
            if (this.IsIpAddressSet(connection.RemoteIpAddress))
            {
                // We have a remote address set up
                return this.IsIpAddressSet(connection.LocalIpAddress)
                    ? connection.RemoteIpAddress.Equals(connection.LocalIpAddress) // Is local is same as remote, then we are local
                    : IPAddress.IsLoopback(connection.RemoteIpAddress); // else we are remote if the remote IP address is not a loopback address
            }

            return true;
        }

        /// <inheritdoc/>
        public virtual string GetRawUrl(HttpRequest request)
        {
            // first try to get the raw target from request feature
            // note: value has not been UrlDecoded
            var rawUrl = request.HttpContext.Features.Get<IHttpRequestFeature>()?.RawTarget;

            // or compose raw URL manually
            if (string.IsNullOrEmpty(rawUrl))
            {
                rawUrl = $"{request.PathBase}{request.Path}{request.QueryString}";
            }

            return rawUrl;
        }

        /// <summary>
        /// Is request available.
        /// </summary>
        /// <returns>bool.</returns>
        protected virtual bool IsRequestAvailable()
        {
            if (this._httpContextAccessor?.HttpContext == null)
            {
                return false;
            }

            try
            {
                if (this._httpContextAccessor.HttpContext.Request == null)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Is ip address set.
        /// </summary>
        /// <param name="address">address.</param>
        /// <returns>bool.</returns>
        protected virtual bool IsIpAddressSet(IPAddress address)
        {
            return address != null && address.ToString() != NullIpAddress;
        }

        /// <summary>
        /// Try write web config.
        /// </summary>
        /// <returns>bool.</returns>
        protected virtual bool TryWriteWebConfig()
        {
            try
            {
                this._fileProvider.SetLastWriteTimeUtc(this._fileProvider.MapPath("~/web.config"), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
