using System;
using System.Collections.Generic;
using System.Linq;
using Blog.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

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
            var host = CreateWebHostBuilder(args).Build();

            try
            {
                var scope = host.Services.CreateScope();

                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                context.Database.EnsureCreated();

                var roles = new List<IdentityRole>
                {
                    new IdentityRole("User"),
                    new IdentityRole("Moderator"),
                    new IdentityRole("Admin")
                };

                if (!context.Roles.Any())
                {
                    roles.ForEach(role => { roleManager.CreateAsync(role).GetAwaiter().GetResult(); });
                }

                if (!context.Users.Any(user => user.UserName.Equals("admin")))
                {
                    var adminUser = new IdentityUser
                    {
                        UserName = "admin",
                        Email = "admin@admin.admin"
                    };

                    userManager.CreateAsync(adminUser, "password");
                    roles.ForEach(role => { userManager.AddToRoleAsync(adminUser, role.Name).GetAwaiter().GetResult(); });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            host.Run();
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
