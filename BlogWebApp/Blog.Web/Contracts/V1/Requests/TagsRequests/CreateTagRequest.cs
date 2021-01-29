namespace Blog.Web.Contracts.V1.Requests.TagsRequests
{
    /// <summary>
    /// Create tag request.
    /// </summary>
    public class CreateTagRequest
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }
    }
}