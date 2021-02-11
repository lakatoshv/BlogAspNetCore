// <copyright file="CommonHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text.RegularExpressions;
    using Blog.Core.Infrastructure;

    /// <summary>
    /// Common Helper.
    /// </summary>
    public class CommonHelper
    {
        // we use EmailValidator from FluentValidation. So let's keep them sync - https://github.com/JeremySkinner/FluentValidation/blob/master/src/FluentValidation/Validators/EmailValidator.cs
        private const string EmailExpression = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-||_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+([a-z]+|\d|-|\.{0,1}|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])?([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$";

        private static readonly Regex EmailRegex;

        static CommonHelper()
        {
            EmailRegex = new Regex(EmailExpression, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Business days.
        /// </summary>
        /// <param name="firstDay">firstDay.</param>
        /// <param name="lastDay">lastDay.</param>
        /// <param name="bankHolidays">bankHolidays.</param>
        /// <returns>int.</returns>
        public static int BusinessDays(DateTime firstDay, DateTime lastDay, params DateTime[] bankHolidays)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            if (firstDay > lastDay)
            {
                throw new ArgumentException("Incorrect last day " + lastDay);
            }

            var span = lastDay - firstDay;
            var businessDays = span.Days + 1;
            var fullWeekCount = businessDays / 7;

            // find out if there are weekends during the time exceedng the full weeks
            if (businessDays > fullWeekCount * 7)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                var firstDayOfWeek = (int)firstDay.DayOfWeek;
                var lastDayOfWeek = (int)lastDay.DayOfWeek;
                if (lastDayOfWeek < firstDayOfWeek)
                {
                    lastDayOfWeek += 7;
                }

                if (firstDayOfWeek <= 6)
                {
                    // Both Saturday and Sunday are in the remaining time interval
                    if (lastDayOfWeek >= 7)
                    {
                        businessDays -= 2;
                    }
                    else if (lastDayOfWeek >= 6)
                    {
                        // Only Saturday is in the remaining time interval
                        businessDays -= 1;
                    }
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)
                {
                    // Only Sunday is in the remaining time interval
                    businessDays -= 1;
                }
            }

            // subtract the weekends during the full weeks in the interval
            businessDays -= fullWeekCount + fullWeekCount;

            // subtract the number of bank holidays during the time interval
            foreach (var bankHoliday in bankHolidays)
            {
                var bh = bankHoliday.Date;
                if (firstDay <= bh && bh <= lastDay)
                {
                    --businessDays;
                }
            }

            return businessDays;
        }

        /// <summary>
        /// Check subscriber email or throw exception.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <returns>string.</returns>
        public static string EnsureSubscriberEmailOrThrow(string email)
        {
            var output = EnsureNotNull(email);
            output = output.Trim();
            output = EnsureMaximumLength(output, 255);

            if (!IsValidEmail(output))
            {
                throw new BlogException("Email is not valid.");
            }

            return output;
        }

        /// <summary>
        /// Check email.
        /// </summary>
        /// <param name="email">email.</param>
        /// <returns>bool.</returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            email = email.Trim();

            return EmailRegex.IsMatch(email);
        }

        /// <summary>
        /// Check Ip address.
        /// </summary>
        /// <param name="ipAddress">ipAddress.</param>
        /// <returns>bool.</returns>
        public static bool IsValidIpAddress(string ipAddress)
        {
            return IPAddress.TryParse(ipAddress, out IPAddress _);
        }

        /// <summary>
        /// Generate random digit code.
        /// </summary>
        /// <param name="length">length.</param>
        /// <returns>string.</returns>
        public static string GenerateRandomDigitCode(int length)
        {
            var random = new Random();
            var str = string.Empty;
            for (var i = 0; i < length; i++)
            {
                str = string.Concat(str, random.Next(10).ToString());
            }

            return str;
        }

        /// <summary>
        /// Generate random integer.
        /// </summary>
        /// <param name="min">min.</param>
        /// <param name="max">max.</param>
        /// <returns>int.</returns>
        public static int GenerateRandomInteger(int min = 0, int max = int.MaxValue)
        {
            var randomNumberBuffer = new byte[10];
            new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
            return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
        }

        /// <summary>
        /// Check maximum length.
        /// </summary>
        /// <param name="str">str.</param>
        /// <param name="maxLength">maxLength.</param>
        /// <param name="postfix">postfix.</param>
        /// <returns>string.</returns>
        public static string EnsureMaximumLength(string str, int maxLength, string postfix = null)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            var pLen = postfix?.Length ?? 0;

            var result = str.Substring(0, maxLength - pLen);
            if (!string.IsNullOrEmpty(postfix))
            {
                result += postfix;
            }

            return result;
        }

        /// <summary>
        /// Check on Numeric.
        /// </summary>
        /// <param name="str">str.</param>
        /// <returns>string.</returns>
        public static string EnsureNumericOnly(string str)
        {
            return string.IsNullOrEmpty(str) ? string.Empty : new string(str.Where(char.IsDigit).ToArray());
        }

        /// <summary>
        /// Check on Null.
        /// </summary>
        /// <param name="str">str.</param>
        /// <returns>string.</returns>
        public static string EnsureNotNull(string str)
        {
            return str ?? string.Empty;
        }

        /// <summary>
        /// Check on null or empty.
        /// </summary>
        /// <param name="stringsToValidate">stringsToValidate.</param>
        /// <returns>bool.</returns>
        public static bool AreNullOrEmpty(params string[] stringsToValidate)
        {
            return stringsToValidate.Any(string.IsNullOrEmpty);
        }

        /// <summary>
        /// Check array.
        /// </summary>
        /// <typeparam name="T">T.</typeparam>
        /// <param name="a1">a1.</param>
        /// <param name="a2">a2.</param>
        /// <returns>bool.</returns>
        public static bool ArraysEqual<T>(T[] a1, T[] a2)
        {
            // also see Enumerable.SequenceEqual(a1, a2);
            if (ReferenceEquals(a1, a2))
            {
                return true;
            }

            if (a1 == null || a2 == null)
            {
                return false;
            }

            if (a1.Length != a2.Length)
            {
                return false;
            }

            var comparer = EqualityComparer<T>.Default;
            return !a1.Where((t, i) => !comparer.Equals(t, a2[i])).Any();
        }

        /// <summary>
        /// Set property.
        /// </summary>
        /// <param name="instance">instance.</param>
        /// <param name="propertyName">propertyName.</param>
        /// <param name="value">value.</param>
        public static void SetProperty(object instance, string propertyName, object value)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            var instanceType = instance.GetType();
            var pi = instanceType.GetProperty(propertyName);
            if (pi == null)
            {
                throw new BlogException("No property '{0}' found on the instance of type '{1}'.", propertyName, instanceType);
            }

            if (!pi.CanWrite)
            {
                throw new BlogException("The property '{0}' on the instance of type '{1}' does not have a setter.", propertyName, instanceType);
            }

            if (value != null && !value.GetType().IsAssignableFrom(pi.PropertyType))
            {
                value = To(value, pi.PropertyType);
            }

            pi.SetValue(instance, value, new object[0]);
        }

        /// <summary>
        /// Convert value.
        /// </summary>
        /// <param name="value">value.</param>
        /// <param name="destinationType">destinationType.</param>
        /// <returns>object.</returns>
        public static object To(object value, Type destinationType)
        {
            return To(value, destinationType, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Convert value.
        /// </summary>
        /// <param name="value">value.</param>
        /// <param name="destinationType">destinationType.</param>
        /// <param name="culture">culture.</param>
        /// <returns>object.</returns>
        public static object To(object value, Type destinationType, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var sourceType = value.GetType();

            var destinationConverter = TypeDescriptor.GetConverter(destinationType);
            if (destinationConverter.CanConvertFrom(value.GetType()))
            {
                return destinationConverter.ConvertFrom(null!, culture, value);
            }

            var sourceConverter = TypeDescriptor.GetConverter(sourceType);
            if (sourceConverter.CanConvertTo(destinationType))
            {
                return sourceConverter.ConvertTo(null, culture, value, destinationType);
            }

            if (destinationType.IsEnum && value is int)
            {
                return Enum.ToObject(destinationType, (int)value);
            }

            if (!destinationType.IsInstanceOfType(value))
            {
                return Convert.ChangeType(value, destinationType, culture);
            }

            return value;
        }

        /// <summary>
        /// Convert value.
        /// </summary>
        /// <typeparam name="T">T.</typeparam>
        /// <param name="value">value.</param>
        /// <returns>Type.</returns>
        public static T To<T>(object value)
        {
            // return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            return (T)To(value, typeof(T));
        }

        /// <summary>
        /// Convert string to enum.
        /// </summary>
        /// <param name="str">str.</param>
        /// <returns>string.</returns>
        public static string ConvertEnum(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            var result = string.Empty;
            foreach (var c in str)
            {
                if (c.ToString() != c.ToString().ToLower())
                {
                    result += " " + c.ToString();
                }
                else
                {
                    result += c.ToString();
                }
            }

            // ensure no spaces (e.g. when the first letter is upper case)
            result = result.TrimStart();
            return result;
        }

        /// <summary>
        /// Set telerik culture.
        /// </summary>
        public static void SetTelerikCulture()
        {
            // little hack here
            // always set culture to 'en-US' (Kendo UI has a bug related to editing decimal values in other cultures)
            var culture = new CultureInfo("en-US");
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        /// <summary>
        /// Get difference in years.
        /// </summary>
        /// <param name="startDate">startDate.</param>
        /// <param name="endDate">endDate.</param>
        /// <returns>int.</returns>
        public static int GetDifferenceInYears(DateTime startDate, DateTime endDate)
        {
            // source: http://stackoverflow.com/questions/9/how-do-i-calculate-someones-age-in-c
            // this assumes you are looking for the western idea of age and not using East Asian reckoning.
            var age = endDate.Year - startDate.Year;
            if (startDate > endDate.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        /// <summary>
        /// Get private field value.
        /// </summary>
        /// <param name="target">target.</param>
        /// <param name="fieldName">fieldName.</param>
        /// <returns>object.</returns>
        public static object GetPrivateFieldValue(object target, string fieldName)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target", "The assignment target cannot be null.");
            }

            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentException("The field name cannot be null or empty.", "fieldName");
            }

            var t = target.GetType();
            FieldInfo fi = null;

            while (t != null)
            {
                fi = t.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

                if (fi != null)
                {
                    break;
                }

                t = t.BaseType;
            }

            if (fi == null)
            {
                throw new Exception($"Field '{fieldName}' not found in type hierarchy.");
            }

            return fi.GetValue(target);
        }
    }
}
