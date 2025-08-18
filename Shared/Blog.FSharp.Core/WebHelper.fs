namespace Blog.FSharp.Core

open System
open System.Net
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Primitives
open Microsoft.Net.Http.Headers
open Microsoft.AspNetCore.StaticFiles
open Microsoft.AspNetCore.WebUtilities
open System.Collections.Generic
open Blog.FSharp.Core.Infrastructure
open Blog.Core.Configuration

type WebHelper(hostingConfig: HostingConfig, httpContextAccessor: IHttpContextAccessor, fileProvider: IShareFileProvider) =
    
    let NullIpAddress = "::1"

    member private this.IsRequestAvailable() =
        match httpContextAccessor.HttpContext with
        | null -> false
        | ctx ->
            try ctx.Request <> null
            with _ -> false

    member private this.IsIpAddressSet(address: IPAddress) =
        address <> null && address.ToString() <> NullIpAddress

    member this.GetCurrentIpAddress() =
        if not (this.IsRequestAvailable()) then ""
        else
            try
                let headers = httpContextAccessor.HttpContext.Request.Headers
                let mutable ip = null

                let forwardedHeaderKey =
                    match hostingConfig.ForwardedHttpHeader with
                    | Some header when not (String.IsNullOrEmpty header) -> header
                    | _ -> "X-FORWARDED-FOR"

                if headers.ContainsKey(forwardedHeaderKey) then
                    ip <- headers.[forwardedHeaderKey].[0].Split(',').[0].Trim()

                if String.IsNullOrEmpty(ip) && httpContextAccessor.HttpContext.Connection.RemoteIpAddress <> null then
                    ip <- httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString()

                if String.Equals(ip, "::1", StringComparison.InvariantCultureIgnoreCase) then
                    ip <- "127.0.0.1"

                match IPAddress.TryParse(ip) with
                | true, parsedIp -> parsedIp.ToString()
                | _ -> if String.IsNullOrEmpty(ip) then "" else ip.Split(':').[0]
            with _ -> ""

    member this.IsCurrentConnectionSecured() =
        if not (this.IsRequestAvailable()) then false
        else
            let req = httpContextAccessor.HttpContext.Request
            if hostingConfig.UseHttpClusterHttps then
                req.Headers.["HTTP_CLUSTER_HTTPS"].ToString().Equals("on", StringComparison.OrdinalIgnoreCase)
            elif hostingConfig.UseHttpXForwardedProto then
                req.Headers.["X-Forwarded-Proto"].ToString().Equals("https", StringComparison.OrdinalIgnoreCase)
            else
                req.IsHttps

    member this.GetStoreLocation(?useSsl: bool) =
        if not (this.IsRequestAvailable()) then "/"
        else
            let ssl = defaultArg useSsl (this.IsCurrentConnectionSecured())
            let hostHeader = httpContextAccessor.HttpContext.Request.Headers.[HeaderNames.Host].[0] |> Option.ofObj |> Option.defaultValue "localhost"
            let scheme = if ssl then Uri.UriSchemeHttps else Uri.UriSchemeHttp
            sprintf "%s://%s/" scheme (hostHeader.TrimEnd('/'))
