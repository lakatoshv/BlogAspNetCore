using System.Threading.Tasks;
using Blog.Contracts.V1.Requests.PostsRequests;
using Blog.Contracts.V1.Responses;

namespace Blog.IntegrationTests.MethodsForTests.Posts.Interfaces;

/// <summary>
/// Posts tests methods interface.
/// </summary>
public interface IPostsTestsMethods
{
    /// <summary>
    /// Creates the post asynchronous.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns>Task.</returns>
    Task<CreatedResponse<int>> CreatePostAsync(CreatePostRequest request);
}