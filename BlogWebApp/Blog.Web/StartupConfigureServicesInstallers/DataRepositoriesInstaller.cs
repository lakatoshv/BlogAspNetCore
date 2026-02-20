namespace Blog.Web.StartupConfigureServicesInstallers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Data;
using Data.Models;
using Data.Repository;

/// <summary>
/// Data repositories installer.
/// </summary>
/// <seealso cref="IInstaller" />
public class DataRepositoriesInstaller : IInstaller
{
    /// <inheritdoc cref="IInstaller"/>
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

        // Dapper repositories
        services.AddTransient<IDapperRepository<Post>, DapperRepository<Post>>();
        services.AddTransient<IDapperRepository<Comment>, DapperRepository<Comment>>();
        services.AddTransient<IDapperRepository<Profile>, DapperRepository<Profile>>();
        services.AddTransient<IDapperRepository<Message>, DapperRepository<Message>>();
        services.AddTransient<IDapperRepository<Tag>, DapperRepository<Tag>>();
        services.AddTransient<IDapperRepository<PostsTagsRelations>, DapperRepository<PostsTagsRelations>>();
    }
}