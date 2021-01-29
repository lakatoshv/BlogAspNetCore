using System.Collections.Generic;
using Blog.Core.Helpers;

namespace Blog.Web.Contracts.V1.Responses.CommentsResponses
{
    /// <summary>
    /// Paged comments response.
    /// </summary>
    public class PagedCommentsResponse
    {
        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        public IList<CommentResponse> Comments { get; set; }

        /// <summary>
        /// Gets or sets the page information.
        /// </summary>
        /// <value>
        /// The page information.
        /// </value>
        public PageInfo PageInfo { get; set; }
    }
}