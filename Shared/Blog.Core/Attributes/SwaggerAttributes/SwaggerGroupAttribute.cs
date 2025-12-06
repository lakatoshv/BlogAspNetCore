// <copyright file="SwaggerGroupAttribute.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Attributes.SwaggerAttributes;

using System;

/// <summary>
/// Swagger controller group attribute.
/// </summary>
/// <seealso cref="System.Attribute" />
/// <inheritdoc cref="Attribute"/>
[AttributeUsage(AttributeTargets.Class)]
public class SwaggerGroupAttribute(string groupName)
    : Attribute
{
    /// <summary>
    /// Gets the name of the group.
    /// </summary>
    /// <value>
    /// The name of the group.
    /// </value>
    public string GroupName { get; } = groupName;
}