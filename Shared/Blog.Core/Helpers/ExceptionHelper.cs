// <copyright file="ExceptionHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Helpers;

using System;
using System.Text;

/// <summary>
/// Exception helper.
/// </summary>
public static class ExceptionHelper
{
    /// <summary>
    /// Gets the messages.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <returns>string.</returns>
    public static string GetMessages(Exception exception) => exception == null ? "Exception is NULL" : ExceptionHelper.GetErrorMessage(exception);

    /// <summary>
    /// Gets the error message.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <returns>string.</returns>
    private static string GetErrorMessage(Exception exception)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(exception.Message);
        if (exception.InnerException != null)
        {
            stringBuilder.AppendLine(ExceptionHelper.GetErrorMessage(exception.InnerException));
        }

        return stringBuilder.ToString();
    }
}