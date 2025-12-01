namespace Blog.Core.Helpers;

using Calabonga.Microservices.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Email validation helper.
/// </summary>
public static class EmailHelper
{
    /// <summary>
    /// Validates emails entries.
    /// </summary>
    /// <param name="emails">emails.</param>
    /// <returns>IEnumerable.</returns>
    public static IEnumerable<string> GetValidEmails(string emails)
    {
        if (string.IsNullOrWhiteSpace(emails))
        {
            return null;
        }

        var split = emails.Split([';', '|', ' ', ','], StringSplitOptions.RemoveEmptyEntries);
        return split.Where(x => x.IsEmail());
    }
}