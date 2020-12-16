using Blog.Data;
using Blog.Data.Models;
using Blog.Data.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Web.StartupConfigureServicesInstallers
{
    /// <summary>
    /// Data repositories installer.
    /// </summary>
    /// <seealso cref="IInstaller" />
    public class DataRepositoriesInstaller : IInstaller
    {
        /// <inheritdoc />
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            // Data repositories
            // services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            // services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddTransient<IRepository<RefreshToken>, Repository<RefreshToken>>();
            services.AddTransient<IRepository<Setting>, Repository<Setting>>();
            services.AddTransient<IRepository<Post>, Repository<Post>>();
            services.AddTransient<IRepository<Comment>, Repository<Comment>>();
            services.AddTransient<IRepository<Profile>, Repository<Profile>>();
            services.AddTransient<IRepository<Message>, Repository<Message>>();
            services.AddTransient<IRepository<Tag>, Repository<Tag>>();
            services.AddTransient<IRepository<PostsTagsRelations>, Repository<PostsTagsRelations>>();
        }
    }
}