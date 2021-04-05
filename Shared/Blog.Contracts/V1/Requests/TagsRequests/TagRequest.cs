namespace Blog.Contracts.V1.Requests.TagsRequests
{
    using Blog.Contracts.V1.Requests.Interfaces;

    /// <summary>
    /// Tag request.
    /// </summary>
    public class TagRequest : IRequest
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