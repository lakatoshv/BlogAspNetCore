namespace AuthService.Core.Configuration
{
    /// <summary>
    /// Hosting config.
    /// </summary>
    public class HostingConfig
    {
        /// <summary>
        /// Gets or sets the forwarded HTTP header.
        /// </summary>
        /// <value>
        /// The forwarded HTTP header.
        /// </value>
        public string ForwardedHttpHeader { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use HTTP cluster HTTPS].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use HTTP cluster HTTPS]; otherwise, <c>false</c>.
        /// </value>
        public bool UseHttpClusterHttps { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use HTTP x forwarded proto].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use HTTP x forwarded proto]; otherwise, <c>false</c>.
        /// </value>
        public bool UseHttpXForwardedProto { get; set; }
    }
}
