// <copyright file="SwaggerOptions.cs" company="Blog.Core">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Configuration;

/// <summary>
/// Swagger options.
/// </summary>
public class SwaggerOptions
{
    /// <summary>
    /// Gets or sets the json route.
    /// </summary>
    /// <value>
    /// The json route.
    /// </value>
    public string JsonRoute { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>
    /// The description.
    /// </value>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the UI endpoint.
    /// </summary>
    /// <value>
    /// The UI endpoint.
    /// </value>
    public string UiEndpoint { get; set; }
}