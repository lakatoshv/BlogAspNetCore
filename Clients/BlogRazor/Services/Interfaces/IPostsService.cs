using Blog.Contracts.V1.Requests.PostsRequests;
using Blog.Contracts.V1.Responses.PostsResponses;
using System.Threading.Tasks;

namespace BlogRazor.Services.Interfaces
{
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

        /// <summary>
        /// Shows the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        Task<PostWithPagedCommentsResponse> Show(int id);
    }
}
