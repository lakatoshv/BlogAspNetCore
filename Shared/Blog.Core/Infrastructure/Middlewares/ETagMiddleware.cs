// <copyright file="ETagMiddleware.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Infrastructure.Middlewares;

using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

/// <summary>
/// ETag middleware from Mads Kristensen.
/// See https://madskristensen.net/blog/send-etag-headers-in-aspnet-core/
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ETagMiddleware"/> class.
/// </remarks>
/// <param name="next">The next.</param>
public class ETagMiddleware(RequestDelegate next)
{
    /// <summary>
    /// The next
    /// </summary>
    private readonly RequestDelegate _next = next;

    /// <summary>
    /// Invokes the asynchronous.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>Task.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var response = context.Response;
        var originalStream = response.Body;

        await using var ms = new MemoryStream();
        response.Body = ms;

        await _next(context);

        if (IsEtagSupported(response))
        {
            var checksum = CalculateChecksum(ms);

            response.Headers[HeaderNames.ETag] = checksum;

            if (context.Request.Headers.TryGetValue(HeaderNames.IfNoneMatch, out var etag) && checksum == etag)
            {
                response.StatusCode = StatusCodes.Status304NotModified;
                return;
            }
        }

        ms.Position = 0;
        await ms.CopyToAsync(originalStream);
    }

    /// <summary>
    /// Determines whether [is etag supported] [the specified response].
    /// </summary>
    /// <param name="response">The response.</param>
    /// <returns>
    ///   <c>true</c> if [is etag supported] [the specified response]; otherwise, <c>false</c>.
    /// </returns>
    private static bool IsEtagSupported(HttpResponse response)
    {
        if (response.StatusCode != StatusCodes.Status200OK)
        {
            return false;
        }

        // The 100kb length limit is not based in science. Feel free to change
        if (response.Body.Length > 100 * 1024)
        {
            return false;
        }

        return !response.Headers.ContainsKey(HeaderNames.ETag);
    }

    /// <summary>
    /// Calculates the checksum.
    /// </summary>
    /// <param name="ms">The ms.</param>
    /// <returns>string.</returns>
    private static string CalculateChecksum(MemoryStream ms)
    {
        using var algo = SHA1.Create();
        ms.Position = 0;
        var bytes = algo.ComputeHash(ms);
        var checksum = $"\"{WebEncoders.Base64UrlEncode(bytes)}\"";

        return checksum;
    }
}