using System.Threading.Tasks;
using Blog.Web.Contracts.V1.Responses;
using Blog.Web.VIewModels.Posts;

namespace Blog.IntegrationTests.MethodsForTests.Posts.Interfaces
{
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
        Task<CreatedResponse<int>> CreatePostAsync(PostViewModel request);
    }
}