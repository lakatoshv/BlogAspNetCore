namespace Blog.Core.Configuration

/// Application-wide blog configurations.
type BlogConfiguration = {
    
    // Error & Response
    DisplayFullErrorStack: bool
    UseResponseCompression: bool
    StaticFilesCacheControl: string option

    // Azure Blob Storage
    AzureBlobStorageConnectionString: string option
    AzureBlobStorageContainerName: string option
    AzureBlobStorageEndPoint: string option

    // Redis
    RedisCachingEnabled: bool
    RedisCachingConnectionString: string option
    PersistDataProtectionKeysToRedis: bool

    // User Agent
    UserAgentStringsPath: string option
    CrawlerOnlyUserAgentStringsPath: string option

    // Installation
    SupportPreviousNopcommerceVersions: bool
    DisableSampleDataDuringInstallation: bool
    UseFastInstallationService: bool
    PluginsIgnoredDuringInstallation: string option

    // Plugins & Startup
    IgnoreStartupTasks: bool
    ClearPluginShadowDirectoryOnStartup: bool
    CopyLockedPluginAssembilesToSubdirectoriesOnStartup: bool
    UseUnsafeLoadAssembly: bool
    UsePluginsShadowCopy: bool
}