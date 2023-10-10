// <copyright file="EnumHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Helpers;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

/// <summary>
/// Enum Helper.
/// </summary>
/// <typeparam name="T">T.</typeparam>
public static class EnumHelper<T>
    where T : struct
{
    /// <summary>
    /// Returns Enum with DisplayNames.
    /// </summary>
    /// <returns>Dictionary.</returns>
    public static Dictionary<T, string> GetValuesWithDisplayNames()
    {
        var type = typeof(T);
        var r = type.GetEnumValues();

        return r.Cast<object>()
            .ToDictionary(element => (T)element, element => GetDisplayValue((T)element));
    }

    /// <summary>
    /// Returns values from enum.
    /// </summary>
    /// <returns>IList.</returns>
    public static IList<T> GetValues() =>
        typeof(T)
            .GetFields(BindingFlags.Static | BindingFlags.Public)
            .Select(fi => (T)Enum.Parse(typeof(T), fi.Name, false))
            .ToList();

    /// <summary>
    /// Parses the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>Type.</returns>
    public static T Parse(string value) =>
        (T)Enum.Parse(typeof(T), value, true);

    /// <summary>
    /// Tries the parse.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>Type.</returns>
    public static T? TryParse(string value)
    {
        if (Enum.TryParse(value, true, out T result))
        {
            return result;
        }

        return null;
    }

    /// <summary>
    /// Gets the names.
    /// </summary>
    /// <returns>IEnumerable.</returns>
    public static IEnumerable<string> GetNames() =>
        typeof(T)
            .GetFields(BindingFlags.Static | BindingFlags.Public)
            .Select(fi => fi.Name)
            .ToList();

    /// <summary>
    /// Returns values from Enum or Resource file if exists.
    /// </summary>
    /// <param name="value">value.</param>
    /// <returns>IList.</returns>
    public static IList<string> GetDisplayValues(Enum value) =>
        GetNames()
            .Select(obj => GetDisplayValue(Parse(obj)))
            .ToList();

    /// <summary>
    /// Gets the display value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>string.</returns>
    public static string GetDisplayValue(T value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString() ?? string.Empty);

        if (fieldInfo is null)
        {
            return string.Empty;
        }

        var descriptionAttributes = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
        if (descriptionAttributes?.Length > 0 && descriptionAttributes[0].ResourceType != null)
        {
            return LookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);
        }

        if (descriptionAttributes == null)
        {
            return string.Empty;
        }

        return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
    }

    private static string LookupResource(Type resourceManagerProvider, string resourceKey)
    {
        foreach (var staticProperty in resourceManagerProvider.GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
        {
            if (staticProperty.PropertyType != typeof(System.Resources.ResourceManager))
            {
                continue;
            }

            var resourceManager = (System.Resources.ResourceManager)staticProperty.GetValue(null, null);

            if (resourceManager != null)
            {
                return resourceManager.GetString(resourceKey);
            }
        }

        return resourceKey; // Fallback with the key name
    }
}