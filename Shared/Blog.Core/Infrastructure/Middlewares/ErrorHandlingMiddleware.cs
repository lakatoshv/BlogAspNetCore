// <copyright file="ErrorHandlingMiddleware.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Infrastructure.Middlewares;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Helpers;

/// <summary>
/// Custom error handler. It allows to view error messages on UI.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ErrorHandlingMiddleware"/> class.
/// </remarks>
/// <param name="next">The next.</param>
public class ErrorHandlingMiddleware(RequestDelegate next)
{
    /// <summary>
    /// The next.
    /// </summary>
    private readonly RequestDelegate _next = next;

    /// <summary>
    /// Invokes the specified context.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>Task.</returns>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Handles the exception asynchronous.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="exception">The exception.</param>
    /// <returns>Task.</returns>
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        try
        {
            var result = JsonConvert.SerializeObject(ExceptionHelper.GetMessages(exception), Formatting.Indented);

            return context.Response.WriteAsync(result?.Length > 4000
                ? "Error message to long. Please use DEBUG in method HandleExceptionAsync to handle a whole of text of the exception"
                : result);
        }
        catch
        {
            return context.Response.WriteAsync($"{exception.Message} For more information please use DEBUG in method HandleExceptionAsync to handle a whole of text of the exception");
        }
    }
}