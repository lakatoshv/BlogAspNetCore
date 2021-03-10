namespace Blog.Contracts.V1.Responses.PostsResponses
{
    using System.Collections.Generic;

    /// <summary>
    /// Paged posts response.
    /// </summary>
    public class PagedPostsResponse
    {
        /// <summary>
        /// Gets or sets posts.
        /// </summary>
        public IList<PostViewResponse> Posts { get; set; }

        /// <summary>
        /// Gets or sets display type.
        /// </summary>
        public string DisplayType { get; set; }

        /// <summary>
        /// Gets or sets page info.
        /// </summary>
        public PageInfoResponse PageInfo { get; set; }
    }
}