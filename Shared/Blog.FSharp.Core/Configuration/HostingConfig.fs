namespace Blog.Core.Configuration

/// Hosting configuration.
type HostingConfig = {
    /// Forwarded HTTP header
    ForwardedHttpHeader: string option

    /// Use HTTP cluster HTTPS
    UseHttpClusterHttps: bool

    /// Use HTTP X-Forwarded-Proto
    UseHttpXForwardedProto: bool
}


