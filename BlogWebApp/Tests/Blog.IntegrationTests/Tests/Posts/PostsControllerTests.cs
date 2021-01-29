using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Blog.Data.Models;
using Blog.IntegrationTests.MethodsForTests.Posts;
using Blog.Web.Contracts.V1;
using Blog.Web.Contracts.V1.Requests.PostsRequests;
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

        /// <summary>
        /// Gets the returns post when post existing database.
        /// </summary>
        [Fact]
        public async Task Get_ReturnsPost_WhenPostExistingDatabase()
        {
            const string name = "created from test";
            // Arrange
            await AuthenticateAsync();
            var post = new CreatePostRequest
            {
                Title = "created from test",
                Description = "created from test",
                Content = "created from test",
                ImageUrl = "created from test",
            };
            var createdPost = await CreatePostAsync(post);

            // Act
            var response =
                await TestClient.GetAsync(ApiRoutes.PostsController.Show.Replace("{id}", createdPost.Id.ToString()));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedPost = await response.Content.ReadAsAsync<Post>();
            returnedPost.Id.Should().Be(createdPost.Id);
            returnedPost.Title.Should().Be(name);
        }
    }
}