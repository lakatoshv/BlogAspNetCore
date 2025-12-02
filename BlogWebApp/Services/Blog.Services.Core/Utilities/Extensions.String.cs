// <copyright file="Extensions.String.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Utilities;

using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

/// <summary>
/// String extensions.
/// </summary>
public static partial class Extensions
{
    private static readonly Regex WebUrlExpression = new(@"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+(:[0-9]+)?|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)", RegexOptions.Singleline | RegexOptions.Compiled);
    private static readonly Regex EmailExpression = new("^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$", RegexOptions.Singleline | RegexOptions.Compiled);
    private static readonly Regex StripHtmlExpression = new("<\\S[^><]*>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

    /// <summary>
    /// Convert string to int.
    /// </summary>
    /// <param name="input">input.</param>
    /// <returns>int.</returns>
    public static int ToInt(this string input)
    {
        // int result = 0;
        int.TryParse(input, out var result);

        return result;
    }

    /// <summary>
    /// Convert string to bool.
    /// </summary>
    /// <param name="input">input.</param>
    /// <returns>bool.</returns>
    public static bool ToBool(this string input)
    {
        // int result = 0;
        bool.TryParse(input, out var result);

        return result;
    }

    /// <summary>
    /// Converts to guid.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>Guid.</returns>
    [DebuggerStepThrough]
    public static Guid ToGuid(this string value)
    {
        Guid.TryParse(value, out var result);

        return result;
    }

    /// <summary>
    /// Determines whether [is web URL].
    /// </summary>
    /// <param name="target">The target.</param>
    /// <returns>
    ///   <c>true</c> if [is web URL] [the specified target]; otherwise, <c>false</c>.
    /// </returns>
    [DebuggerStepThrough]
    public static bool IsWebUrl(this string target) =>
        !string.IsNullOrEmpty(target) && WebUrlExpression.IsMatch(target);

    /// <summary>
    /// Determines whether this instance is email.
    /// </summary>
    /// <param name="target">The target.</param>
    /// <returns>
    ///   <c>true</c> if the specified target is email; otherwise, <c>false</c>.
    /// </returns>
    [DebuggerStepThrough]
    public static bool IsEmail(this string target) =>
        !string.IsNullOrEmpty(target) && EmailExpression.IsMatch(target);

    /// <summary>
    /// Nulls the safe.
    /// </summary>
    /// <param name="target">The target.</param>
    /// <returns>string.</returns>
    [DebuggerStepThrough]
    public static string NullSafe(this string target) =>
        (target ?? string.Empty).Trim();

    /// <summary>
    /// Formats the with.
    /// </summary>
    /// <param name="target">The target.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>string.</returns>
    [DebuggerStepThrough]
    public static string FormatWith(this string target, params object[] args) =>
        target == null
            ? string.Empty
            : string.Format(CultureInfo.CurrentCulture, target, args);

    /// <summary>
    /// Strips the HTML.
    /// </summary>
    /// <param name="target">The target.</param>
    /// <returns>string.</returns>
    [DebuggerStepThrough]
    public static string StripHtml(this string target) =>
        StripHtmlExpression.Replace(target, string.Empty);

    /// <summary>
    /// Converts to enum.
    /// </summary>
    /// <typeparam name="T">T.</typeparam>
    /// <param name="target">The target.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>Type.</returns>
    [DebuggerStepThrough]
    public static T ToEnum<T>(this string target, T defaultValue)
        where T : IComparable, IFormattable
    {
        var convertedValue = defaultValue;

        if (string.IsNullOrEmpty(target))
        {
            return convertedValue;
        }

        try
        {
            convertedValue = (T)Enum.Parse(typeof(T), target.Trim(), true);
        }
        catch (ArgumentException)
        {
        }

        return convertedValue;
    }
}