using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Blog.Data.Models;
using Blog.IntegrationTests.MethodsForTests.Posts;
using Blog.Web.Contracts.V1;
using FluentAssertions;
using Xunit;

namespace Blog.IntegrationTests.Tests.Posts
{
    /// <summary>
    /// Posts controller tests.
    /// </summary>
    /// <seealso cref="PostsTestsMethods" />
    public class PostsControllerTests : PostsTestsMethods 
    {
        /// <summary>
        /// Gets all without any posts returns empty response.
        /// </summary>
        [Fact]
        public async Task GetAll_WithoutAnyPosts_ReturnsEmptyResponse()
        {
            // Arrange
            await AuthenticateAsync();

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.PostsController.GetPosts); 

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<List<Post>>()).Should().BeEmpty();
        }
    }
}