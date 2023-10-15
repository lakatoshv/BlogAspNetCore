using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Blog.Data;
using Blog.IntegrationTests.General.Interfaces;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Requests.UsersRequests;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blog.IntegrationTests.General;

/// <summary>
/// General integration test.
/// </summary>
public class GeneralIntegrationTest : IGeneralIntegrationTest
{
    /// <summary>
    /// The test client.
    /// </summary>
    protected readonly HttpClient TestClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="GeneralIntegrationTest"/> class.
    /// </summary>
    protected GeneralIntegrationTest()
    {
        var appFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(ApplicationDbContext));

                    // Put database in memory.
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("BlogAspNetCoreNew");
                    });

                });
            });
        TestClient = appFactory.CreateClient();
    }

    /// <inheritdoc cref="IGeneralIntegrationTest"/>
    public async Task AuthenticateAsync()
    {
        TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
    }

    /// <summary>
    /// Gets the JWT asynchronous.
    /// </summary>
    /// <returns>Task.</returns>
    private async Task<string> GetJwtAsync()
    {
        var user = new RegistrationRequest
        {
            Email = "test@integration.com",
            Password = "123123",
        };
        var registerResponse = await TestClient.PostAsJsonAsync(ApiRoutes.AccountsController.Register, user);

        if (registerResponse.IsSuccessStatusCode)
        {
            var loginResponse = await TestClient.PostAsJsonAsync(ApiRoutes.AccountsController.Login,
                new LoginRequest
                {
                    Email = user.Email,
                    Password = user.Password,
                });

            var jwt = await loginResponse.Content.ReadAsStringAsync();

            return jwt;
        }
        else
        {
            var loginResponse = await TestClient.PostAsJsonAsync(ApiRoutes.AccountsController.Login,
                new LoginRequest
                {
                    Email = user.Email,
                    Password = user.Password,
                });

            var jwt = await loginResponse.Content.ReadAsStringAsync();

            return jwt;
        }
    }
}