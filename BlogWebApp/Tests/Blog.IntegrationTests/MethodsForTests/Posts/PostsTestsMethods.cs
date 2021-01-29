using System.Net.Http;
using System.Threading.Tasks;
using Blog.IntegrationTests.General;
using Blog.IntegrationTests.MethodsForTests.Posts.Interfaces;
using Blog.Web.Contracts.V1;
using Blog.Web.Contracts.V1.Requests.PostsRequests;
using Blog.Web.Contracts.V1.Responses;

namespace Blog.IntegrationTests.MethodsForTests.Posts
{
    /// <summary>
    /// Posts tests methods.
    /// </summary>
    /// <seealso cref="GeneralIntegrationTest" />
    /// <seealso cref="IPostsTestsMethods" />
    public class PostsTestsMethods : GeneralIntegrationTest, IPostsTestsMethods
    {
        /// <inheritdoc cref="IPostsTestsMethods"/>
        public async Task<CreatedResponse<int>> CreatePostAsync(CreatePostRequest request)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.PostsController.Posts, request);

            return await response.Content.ReadAsAsync<CreatedResponse<int>>();
        }
    }
}