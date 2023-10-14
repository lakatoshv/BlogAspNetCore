using Blazor.Contracts.V1.Requests.PostsRequests;
using Blazor.Contracts.V1.Responses.PostsResponses;
using Blazor.Sdk.V1;
using System.Threading.Tasks;

namespace BlogBlazor.Services.Interfaces;

/// <summary>
/// Posts service interface.
/// </summary>
public interface IPostsService
{
    /// <summary>
    /// Gets the posts.
    /// </summary>
    /// <param name="searchParameters">The search parameters.</param>
    /// <returns>Task.</returns>
    Task<PagedPostsResponse> GetPosts(PostsSearchParametersRequest searchParameters);

    Task<PostWithPagedCommentsResponse> GetPost(int postId);
}