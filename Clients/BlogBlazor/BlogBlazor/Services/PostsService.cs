namespace BlogBlazor.Services;

using Blazor.Contracts.V1.Requests.PostsRequests;
using Blazor.Contracts.V1.Responses.PostsResponses;
using Blazor.Sdk.V1;
using Constants;
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
    public async Task<PostWithPagedCommentsResponse> GetPost(int postId)
        => await _postsControllerRequestsApi.Show(postId);
}