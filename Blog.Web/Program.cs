using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Blog.Web
{
    /// <summary>
    /// Start point in application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Start point in application.
        /// </summary>
        /// <param name="args">args.</param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create web host builder.
        /// </summary>
        /// <param name="args">args.</param>
        /// <returns>IWebHostBuilder.</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
