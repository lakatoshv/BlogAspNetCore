using Blog.EntityServices.DapperServices.Interfaces;

namespace Blog.Web.StartupConfigureServicesInstallers;

using Blog.EntityServices.DapperServices;
using Blog.Services.Core.Caching;
using Blog.Services.Core.Caching.Interfaces;
using Blog.Services.Core.Email.Templates;
using Blog.Services.Core.Security;
using CommonServices;
using CommonServices.EmailServices;
using CommonServices.EmailServices.Interfaces;
using CommonServices.Interfaces;
using Core;
using Core.Configuration;
using Core.Infrastructure;
using Core.Interfaces;
using Data;
using Data.Models;
using Data.Repository;
using EntityServices.ControllerContext;
using EntityServices.EntityFrameworkServices;
using EntityServices.EntityFrameworkServices.Identity.Auth;
using EntityServices.EntityFrameworkServices.Identity.RefreshToken;
using EntityServices.EntityFrameworkServices.Identity.Registration;
using EntityServices.EntityFrameworkServices.Identity.User;
using EntityServices.Interfaces;
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

        // Dapper services
        services.AddTransient<IPostsDapperService, PostsDapperService>();
        services.AddTransient<ICommentsDapperService, CommentsDapperService>();
        services.AddTransient<IProfileDapperService, ProfileDapperService>();
        services.AddTransient<IMessagesDapperService, MessagesDapperService>();
        services.AddTransient<ITagsDapperService, TagsDapperService>();
        services.AddTransient<IPostsTagsRelationsDapperService, PostsTagsRelationsDapperService>();
    }
}