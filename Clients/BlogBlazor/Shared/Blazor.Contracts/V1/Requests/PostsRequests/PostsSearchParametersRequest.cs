namespace Blazor.Contracts.V1.Requests.PostsRequests
{
    /// <summary>
    /// Posts search parameters request.
    /// </summary>
    /// <seealso cref="SearchParametersRequest" />
    public class PostsSearchParametersRequest : SearchParametersRequest
    {
        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public string Tag { get; set; }
    }
}