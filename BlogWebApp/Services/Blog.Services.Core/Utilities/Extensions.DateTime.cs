// <copyright file="Extensions.DateTime.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Utilities;

using System;

/// <summary>
/// DateTime extensions.
/// </summary>
public static partial class Extensions
{
    /// <summary>
    /// Convert DateTime to unix time stamp.
    /// </summary>
    /// <param name="dateTime">dateTime.</param>
    /// <returns>long.</returns>
    public static long ToUnixTimeStamp(this DateTime dateTime)
    {
        var offset = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var universalTime = dateTime.ToUniversalTime();

        var delta = Math.Round((universalTime - offset).TotalSeconds);

        return (long)delta;
    }

    /// <summary>
    /// Convert DateTime to user time.
    /// </summary>
    /// <param name="dt">dt.</param>
    /// <param name="sourceDateTimeKind">sourceDateTimeKind.</param>
    /// <returns>DateTime.</returns>
    public static DateTime ConvertToUserTime(this DateTime dt, DateTimeKind sourceDateTimeKind)
    {
        dt = DateTime.SpecifyKind(dt, sourceDateTimeKind);
        if (sourceDateTimeKind == DateTimeKind.Local && TimeZoneInfo.Local.IsInvalidTime(dt))
        {
            return dt;
        }

        // TODO Get customer\user current time zone
        // var currentUserTimeZoneInfo = CurrentTimeZone;
        return TimeZoneInfo.ConvertTime(dt, TimeZoneInfo.Utc);
    }

    /// <summary>
    /// Convert DateTime to string.
    /// </summary>
    /// <param name="dateTime">dateTime.</param>
    /// <returns>string.</returns>
    public static string ToSafeFileName(this DateTime dateTime)
    {
        return $"{dateTime.Day:00}{dateTime.Month:00}{dateTime.Year}";
    }
}