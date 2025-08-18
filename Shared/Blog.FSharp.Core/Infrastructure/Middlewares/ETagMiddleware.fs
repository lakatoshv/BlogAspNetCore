namespace Blog.FSharp.Core.Infrastructure.Middlewares

open System.IO
open System.Security.Cryptography
open System.Threading.Tasks
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.WebUtilities
open Microsoft.Net.Http.Headers

/// <summary>
/// ETag middleware from Mads Kristensen.
/// See https://madskristensen.net/blog/send-etag-headers-in-aspnet-core/
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ETagMiddleware"/> class.
/// </remarks>
type ETagMiddleware(next: RequestDelegate) =

    /// <summary>
    /// The next middleware in the pipeline.
    /// </summary>
    let _next = next

    /// <summary>
    /// Invokes the asynchronous operation.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    member _.InvokeAsync(context: HttpContext) : Task =
        task {
            let response = context.Response
            let originalStream = response.Body

            use ms = new MemoryStream()
            response.Body <- ms

            do! _next.Invoke(context)

            if ETagMiddleware.IsEtagSupported(response) then
                let checksum = ETagMiddleware.CalculateChecksum(ms)

                response.Headers[HeaderNames.ETag] <- checksum

                match context.Request.Headers.TryGetValue(HeaderNames.IfNoneMatch) with
                | true, etag when checksum = etag ->
                    response.StatusCode <- StatusCodes.Status304NotModified
                    return ()
                | _ -> ()

            ms.Position <- 0
            do! ms.CopyToAsync(originalStream)
        }

    /// <summary>
    /// Determines whether ETag is supported for the specified response.
    /// </summary>
    /// <param name="response">The response.</param>
    /// <returns><c>true</c> if ETag is supported; otherwise, <c>false</c>.</returns>
    static member private IsEtagSupported(response: HttpResponse) : bool =
        if response.StatusCode <> StatusCodes.Status200OK then
            false
        elif response.Body.Length > 100L * 1024L then
            // The 100kb length limit is arbitrary; feel free to change.
            false
        else
            not (response.Headers.ContainsKey(HeaderNames.ETag))

    /// <summary>
    /// Calculates the checksum of the response body.
    /// </summary>
    /// <param name="ms">The memory stream containing the response body.</param>
    /// <returns>A string representing the checksum.</returns>
    static member private CalculateChecksum(ms: MemoryStream) : string =
        use algo = SHA1.Create()
        ms.Position <- 0
        let bytes = algo.ComputeHash(ms)
        let checksum = "\"" + WebEncoders.Base64UrlEncode(bytes) + "\""
        checksum