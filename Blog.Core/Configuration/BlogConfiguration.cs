namespace Blog.Core.Configuration
{
    public class BlogConfiguration
    {
        public bool DisplayFullErrorStack { get; set; }

        public string StaticFilesCacheControl { get; set; }

        public bool UseResponseCompression { get; set; }

        public string AzureBlobStorageConnectionString { get; set; }

        public string AzureBlobStorageContainerName { get; set; }

        public string AzureBlobStorageEndPoint { get; set; }

        public bool RedisCachingEnabled { get; set; }

        public string RedisCachingConnectionString { get; set; }

        public bool PersistDataProtectionKeysToRedis { get; set; }

        public string UserAgentStringsPath { get; set; }

        public string CrawlerOnlyUserAgentStringsPath { get; set; }

        public bool SupportPreviousNopcommerceVersions { get; set; }

        public bool DisableSampleDataDuringInstallation { get; set; }

        public bool UseFastInstallationService { get; set; }

        public string PluginsIgnoredDuringInstallation { get; set; }

        public bool IgnoreStartupTasks { get; set; }

        public bool ClearPluginShadowDirectoryOnStartup { get; set; }

        public bool CopyLockedPluginAssembilesToSubdirectoriesOnStartup { get; set; }

        public bool UseUnsafeLoadAssembly { get; set; }

        public bool UsePluginsShadowCopy { get; set; }
    }
}
