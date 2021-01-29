using System.Net.Http;
using System.Threading.Tasks;
using Blog.Web;
using Blog.Contracts.V1;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Blog.IntegrationTests
{
    public class UnitTest1
    {
        private readonly HttpClient _client;
        public UnitTest1()
        {
            var appFactory = new WebApplicationFactory<Startup>();
            _client = appFactory.CreateClient();
        }

        [Fact]
        public async Task Test1()
        {
            var post = await _client.GetAsync(ApiRoutes.PostsController.Show.Replace("{id}", "1")); 
        }
    }
}
