namespace Blog.Core.Configuration

/// Redis cache configuration.
type RedisCacheConfiguration = {
    /// <c>true</c> if enabled; otherwise, <c>false</c>.
    Enabled: bool

    /// The Redis connection string.
    ConnectionString: string option
}