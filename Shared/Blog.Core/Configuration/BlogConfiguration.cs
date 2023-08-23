// <copyright file="BlogConfiguration.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Configuration;

/// <summary>
/// Blog configurations.
/// </summary>
public class BlogConfiguration
{
    /// <summary>
    /// Gets or sets a value indicating whether Display or not full error message.
    /// </summary>
    public bool DisplayFullErrorStack { get; set; }

    /// <summary>
    /// Gets or sets staticFilesCacheControl.
    /// </summary>
    public string StaticFilesCacheControl { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether useResponseCompression.
    /// </summary>
    public bool UseResponseCompression { get; set; }

    /// <summary>
    /// Gets or sets azureBlobStorageConnectionString.
    /// </summary>
    public string AzureBlobStorageConnectionString { get; set; }

    /// <summary>
    /// Gets or sets azureBlobStorageContainerName.
    /// </summary>
    public string AzureBlobStorageContainerName { get; set; }

    /// <summary>
    /// Gets or sets azureBlobStorageEndPoint.
    /// </summary>
    public string AzureBlobStorageEndPoint { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether redisCachingEnabled.
    /// </summary>
    public bool RedisCachingEnabled { get; set; }

    /// <summary>
    /// Gets or sets redisCachingConnectionString.
    /// </summary>
    public string RedisCachingConnectionString { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether persistDataProtectionKeysToRedis.
    /// </summary>
    public bool PersistDataProtectionKeysToRedis { get; set; }

    /// <summary>
    /// Gets or sets userAgentStringsPath.
    /// </summary>
    public string UserAgentStringsPath { get; set; }

    /// <summary>
    /// Gets or sets crawlerOnlyUserAgentStringsPath.
    /// </summary>
    public string CrawlerOnlyUserAgentStringsPath { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether supportPreviousNopcommerceVersions.
    /// </summary>
    public bool SupportPreviousNopcommerceVersions { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether supportPreviousNopcommerceVersions.
    /// </summary>
    public bool DisableSampleDataDuringInstallation { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether useFastInstallationService.
    /// </summary>
    public bool UseFastInstallationService { get; set; }

    /// <summary>
    /// Gets or sets pluginsIgnoredDuringInstallation.
    /// </summary>
    public string PluginsIgnoredDuringInstallation { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether ignoreStartupTasks.
    /// </summary>
    public bool IgnoreStartupTasks { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether clearPluginShadowDirectoryOnStartup.
    /// </summary>
    public bool ClearPluginShadowDirectoryOnStartup { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether copyLockedPluginAssembilesToSubdirectoriesOnStartup.
    /// </summary>
    public bool CopyLockedPluginAssembilesToSubdirectoriesOnStartup { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether useUnsafeLoadAssembly.
    /// </summary>
    public bool UseUnsafeLoadAssembly { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether usePluginsShadowCopy.
    /// </summary>
    public bool UsePluginsShadowCopy { get; set; }
}