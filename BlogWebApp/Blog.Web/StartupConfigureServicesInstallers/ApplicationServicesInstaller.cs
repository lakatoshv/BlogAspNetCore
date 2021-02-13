namespace Blog.Web.StartupConfigureServicesInstallers
{
    using Blog.Core;
    using Blog.Core.Configuration;
    using Blog.Core.Infrastructure;
    using Blog.Core.Interfaces;
    using Blog.Data;
    using Blog.Data.Models;
    using Blog.Data.Repository;
    using Blog.Services;
    using Blog.Services.CacheServices;
    using Blog.Services.ControllerContext;
    using Blog.Services.Core.Caching;
    using Blog.Services.Core.Caching.Interfaces;
    using Blog.Services.Core.Email.Templates;
    using Blog.Services.Core.Security;
    using Blog.Services.EmailServices;
    using Blog.Services.EmailServices.Interfaces;
    using Blog.Services.Identity.Auth;
    using Blog.Services.Identity.RefreshToken;
    using Blog.Services.Identity.Registration;
    using Blog.Services.Identity.User;
    using Blog.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Application services installer.
    /// </summary>
    /// <seealso cref="IInstaller" />
    public class ApplicationServicesInstaller : IInstaller
    {
        /// <inheritdoc cref="IInstaller"/>
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(x => x.GetService<IOptions<BlogConfiguration>>()?.Value);
            services.AddTransient<HostingConfig>();

            services.AddTransient<IStaticCacheManager, MemoryCacheManager>();
            //services.AddTransient<IDbContext, ApplicationDbContext>();
            services.AddTransient<IWebHelper, WebHelper>();
            services.AddTransient<IShareFileProvider, FileProvider>();

            services.AddTransient<IControllerContext, MyControllerContext>();
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
            services.AddHttpContextAccessor();

            // Application services
            services.AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, ApplicationRoleStore>();
            services.AddTransient<IRepository<ApplicationUser>, Repository<ApplicationUser>>();

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IEmailExtensionService, EmailExtensionService>();

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IRefreshTokenService, RefreshTokenService>();
            services.AddTransient<IRegistrationService, RegistrationService>();
            services.AddTransient<IEmailTemplateProvider, SimpleEmailTemplateProvider>();

            services.AddTransient<IPostsService, PostsService>();
            services.AddTransient<ICommentsService, CommentsService>();
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<IMessagesService, MessagesService>();
            services.AddTransient<ITagsService, TagsService>();
            services.AddTransient<IPostsTagsRelationsService, PostsTagsRelationsService>();

            services.AddTransient<IResponseCacheService, ResponseCacheService>();
        }
    }
}