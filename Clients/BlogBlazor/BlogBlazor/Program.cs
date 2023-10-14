using BlogBlazor.Constants;
using BlogBlazor.Services;
using BlogBlazor.Services.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlogBlazor;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");

        builder.Services.AddScoped<IPostsService, PostsService>();
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(ApiUrls.BlogApiUrl) });

        await builder.Build().RunAsync();
    }
}