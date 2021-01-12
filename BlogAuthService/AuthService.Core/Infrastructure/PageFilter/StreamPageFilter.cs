namespace AuthService.Core.Infrastructure.PageFilter
{
    /// <summary>
    /// Stream page filter.
    /// </summary>
    public class StreamPageFilter
    {
        /// <summary>
        /// Gets or sets the previous value.
        /// </summary>
        /// <value>
        /// The previous value.
        /// </value>
        public int PreviousValue { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets the page count.
        /// </summary>
        /// <value>
        /// The page count.
        /// </value>
        public int PageCount { get; set; }
    }
}
