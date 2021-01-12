namespace AuthService.Core.Emails
{
    /// <summary>
    /// Email extension options.
    /// </summary>
    public class EmailExtensionOptions
    {
        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        /// <value>
        /// The base URL.
        /// </value>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        public string From { get; set; }
    }
}
