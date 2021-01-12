namespace AuthService.Core.Configuration
{
    /// <summary>
    /// Blog configuration.
    /// </summary>
    public class BlogConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether [display full error stack].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [display full error stack]; otherwise, <c>false</c>.
        /// </value>
        public bool DisplayFullErrorStack { get; set; }

        /// <summary>
        /// Gets or sets the static files cache control.
        /// </summary>
        /// <value>
        /// The static files cache control.
        /// </value>
        public string StaticFilesCacheControl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use response compression].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use response compression]; otherwise, <c>false</c>.
        /// </value>
        public bool UseResponseCompression { get; set; }

        /// <summary>
        /// Gets or sets the azure BLOB storage connection string.
        /// </summary>
        /// <value>
        /// The azure BLOB storage connection string.
        /// </value>
        public string AzureBlobStorageConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the name of the azure BLOB storage container.
        /// </summary>
        /// <value>
        /// The name of the azure BLOB storage container.
        /// </value>
        public string AzureBlobStorageContainerName { get; set; }

        /// <summary>
        /// Gets or sets the azure BLOB storage end point.
        /// </summary>
        /// <value>
        /// The azure BLOB storage end point.
        /// </value>
        public string AzureBlobStorageEndPoint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [redis caching enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [redis caching enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool RedisCachingEnabled { get; set; }

        /// <summary>
        /// Gets or sets the redis caching connection string.
        /// </summary>
        /// <value>
        /// The redis caching connection string.
        /// </value>
        public string RedisCachingConnectionString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [persist data protection keys to redis].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [persist data protection keys to redis]; otherwise, <c>false</c>.
        /// </value>
        public bool PersistDataProtectionKeysToRedis { get; set; }

        /// <summary>
        /// Gets or sets the user agent strings path.
        /// </summary>
        /// <value>
        /// The user agent strings path.
        /// </value>
        public string UserAgentStringsPath { get; set; }

        /// <summary>
        /// Gets or sets the crawler only user agent strings path.
        /// </summary>
        /// <value>
        /// The crawler only user agent strings path.
        /// </value>
        public string CrawlerOnlyUserAgentStringsPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [support previous nopcommerce versions].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [support previous nopcommerce versions]; otherwise, <c>false</c>.
        /// </value>
        public bool SupportPreviousNopcommerceVersions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [disable sample data during installation].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [disable sample data during installation]; otherwise, <c>false</c>.
        /// </value>
        public bool DisableSampleDataDuringInstallation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use fast installation service].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use fast installation service]; otherwise, <c>false</c>.
        /// </value>
        public bool UseFastInstallationService { get; set; }

        /// <summary>
        /// Gets or sets the plugins ignored during installation.
        /// </summary>
        /// <value>
        /// The plugins ignored during installation.
        /// </value>
        public string PluginsIgnoredDuringInstallation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [ignore startup tasks].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [ignore startup tasks]; otherwise, <c>false</c>.
        /// </value>
        public bool IgnoreStartupTasks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [clear plugin shadow directory on startup].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [clear plugin shadow directory on startup]; otherwise, <c>false</c>.
        /// </value>
        public bool ClearPluginShadowDirectoryOnStartup { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [copy locked plugin assembiles to subdirectories on startup].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [copy locked plugin assembiles to subdirectories on startup]; otherwise, <c>false</c>.
        /// </value>
        public bool CopyLockedPluginAssembilesToSubdirectoriesOnStartup { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use unsafe load assembly].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use unsafe load assembly]; otherwise, <c>false</c>.
        /// </value>
        public bool UseUnsafeLoadAssembly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use plugins shadow copy].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use plugins shadow copy]; otherwise, <c>false</c>.
        /// </value>
        public bool UsePluginsShadowCopy { get; set; }
    }
}
