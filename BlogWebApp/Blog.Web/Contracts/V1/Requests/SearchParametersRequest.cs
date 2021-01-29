namespace Blog.Web.Contracts.V1.Requests
{
    /// <summary>
    /// Search parameters request.
    /// </summary>
    public class SearchParametersRequest
    {
        /// <summary>
        /// Gets or sets search.
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        /// Gets or sets the sort parameters.
        /// </summary>
        /// <value>
        /// The sort parameters.
        /// </value>
        public SortParametersRequest SortParameters { get; set; }
    }
}