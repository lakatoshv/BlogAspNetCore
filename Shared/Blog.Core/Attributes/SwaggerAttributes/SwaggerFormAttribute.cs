// <copyright file="SwaggerFormAttribute.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Attributes.SwaggerAttributes;

using System;

/// <summary>
/// Custom attribute for Swagger Upload Form.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SwaggerFormAttribute"/> class.
/// Custom attribute for Swagger Upload Form.
/// </remarks>
/// <seealso cref="Attribute" />
/// <inheritdoc cref="Attribute"/>
[AttributeUsage(AttributeTargets.Method)]
public sealed class SwaggerFormAttribute(string parameterName, string description, bool hasFileUpload = true)
    : Attribute
{
    /// <summary>
    /// Gets a value indicating whether this instance has file upload.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance has file upload; otherwise, <c>false</c>.
    /// </value>
    public bool HasFileUpload { get; } = hasFileUpload;

    /// <summary>
    /// Gets the name of the parameter <see cref="T:IFormFile"/>.
    /// </summary>
    /// <value>
    /// The name of the parameter.
    /// </value>
    public string ParameterName { get; } = parameterName;

    /// <summary>
    /// Gets the description.
    /// </summary>
    /// <value>
    /// The description.
    /// </value>
    public string Description { get; } = description;
}