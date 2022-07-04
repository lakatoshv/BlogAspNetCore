namespace BlogRazor.Services
{
    using Blog.Contracts.V1.Requests.PostsRequests;
    using Blog.Contracts.V1.Responses.PostsResponses;
    using Blog.Sdk.V1;
    using BlogRazor.Constants;
    using Interfaces;
    using Refit;
    using System.Threading.Tasks;

    /// <summary>
    /// Posts service.
    /// </summary>
    /// <seealso cref="IPostsService" />
    public class PostsService : IPostsService
    {
        /// <summary>
        /// The posts controller requests API.
        /// </summary>
        private readonly IPostsControllerRequestsApi _postsControllerRequestsApi;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostsService"/> class.
        /// </summary>
        public PostsService()
            => _postsControllerRequestsApi = RestService.For<IPostsControllerRequestsApi>(ApiUrls.BlogApiUrl);

        /// <inheritdoc cref="IPostsService"/>
        public async Task<PagedPostsResponse> GetPosts(PostsSearchParametersRequest searchParameters)
            => await _postsControllerRequestsApi.GetPosts(searchParameters);

        /// <inheritdoc cref="IPostsService"/>
        public async Task<PostWithPagedCommentsResponse> Show(int id)
            => await _postsControllerRequestsApi.Show(id);
    }
}
