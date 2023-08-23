// <copyright file="HostingConfig.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Configuration;

/// <summary>
/// Hosting configuration.
/// </summary>
public class HostingConfig
{
    /// <summary>
    /// Gets or sets forwardedHttpHeader.
    /// </summary>
    public string ForwardedHttpHeader { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether useHttpClusterHttps.
    /// </summary>
    public bool UseHttpClusterHttps { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether useHttpXForwardedProto.
    /// </summary>
    public bool UseHttpXForwardedProto { get; set; }
}