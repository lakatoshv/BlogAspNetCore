using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers.V1
{
    /*
    public class HomeController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(
            IControllerContext controllerContext,
            IHttpClientFactory httpClientFactory
        ) : base(controllerContext)
        {
            _httpClientFactory = httpClientFactory; 
        }

        public async Task<ActionResult> Index()
        {
            // retrive access token
            var serverClient = _httpClientFactory.CreateClient();
            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:44312/");
            var tokenResponce = await serverClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = "client1",
                ClientSecret = "password",
                Scope = "BlogApi",
            });
            // retrive secret data
            var apiClient = _httpClientFactory.CreateClient();
            apiClient.SetBearerToken(tokenResponce.AccessToken);
            var response =await apiClient.GetAsync("https://localhost:44377/secret");
            var content = await response.Content.ReadAsStringAsync();

            return Ok(new
            {
                access_token = tokenResponce.AccessToken,
                message = content,
            });
        }
    }
    */
}