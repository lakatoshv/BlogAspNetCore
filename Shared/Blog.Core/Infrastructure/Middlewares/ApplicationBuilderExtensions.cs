// <copyright file="ApplicationBuilderExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Infrastructure.Middlewares;

using Microsoft.AspNetCore.Builder;

/// <summary>
/// ETagger extension.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Uses the e tagger.
    /// </summary>
    /// <param name="app">The application.</param>
    public static void UseETagger(this IApplicationBuilder app)
    {
        app.UseMiddleware<ETagMiddleware>();
    }
}