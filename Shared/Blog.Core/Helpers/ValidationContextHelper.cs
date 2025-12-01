// <copyright file="ValidationContextHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Helpers;

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// ValidationContext Helper for validation operations.
/// </summary>
public static class ValidationContextHelper
{
    /// <summary>
    /// Tries to validate.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="results">The results.</param>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>bool.</returns>
    public static bool TryValidate(object obj, out Collection<ValidationResult> results, ValidationContext validationContext = null)
    {
        var context = validationContext ?? new ValidationContext(obj, serviceProvider: null, items: null);
        results = [];

        return Validator.TryValidateObject(obj, context, results, validateAllProperties: true);
    }
}