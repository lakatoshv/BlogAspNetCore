namespace Blazor.Contracts.V1.Responses.TagsResponses
{
    using System.Collections.Generic;

    /// <summary>
    /// Paged tags response.
    /// </summary>
    public class PagedTagsResponse
    {
        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public IList<TagResponse> Tags { get; set; }

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