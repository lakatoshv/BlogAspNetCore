namespace Blazor.Contracts.V1.Requests
{
    using Blazor.Contracts.V1.Requests.Interfaces;

    /// <summary>
    /// Sort Parameters request.
    /// </summary>
    public class SortParametersRequest : IRequest
    {
        /// <summary>
        /// Gets or sets orderBy.
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// Gets or sets sortBy.
        /// </summary>
        public string SortBy { get; set; }

        /// <summary>
        /// Gets or sets currentPage.
        /// </summary>
        public int? CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets pageSize.
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// Gets or sets displayType.
        /// </summary>
        public string DisplayType { get; set; }
    }
}