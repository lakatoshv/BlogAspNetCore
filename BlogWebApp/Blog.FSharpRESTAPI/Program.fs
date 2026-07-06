open Blog.Core.Configuration
open Blog.Services.Core.Caching.Interfaces
open Blog.Services.Core.Caching
open Blog.Core.Interfaces
open Blog.Core
open Blog.Core.Infrastructure
open Blog.EntityServices.ControllerContext
open Microsoft.AspNetCore.Authorization
open Blog.Services.Core.Security
open Microsoft.AspNetCore.Identity
open Blog.EntityServices.EntityFrameworkServices.Identity.Auth
open Blog.EntityServices.EntityFrameworkServices.Identity.User
open Blog.CommonServices.EmailServices.Interfaces
open Blog.EntityServices.EntityFrameworkServices.Identity.RefreshToken
open Blog.EntityServices.EntityFrameworkServices.Identity.Registration
open Blog.Services.Core.Email.Templates
open Blog.EntityServices.Interfaces
open Blog.EntityServices.EntityFrameworkServices
open Blog.CommonServices.Interfaces
open Blog.CommonServices
open Blog.CommonServices.EmailServices
open Microsoft.Extensions.Options
open Blog.Services.Core.Email.Smtp
open Blog.Core.Emails
open Blog.Services.Core.Identity.Auth
open Microsoft.Extensions.Diagnostics.HealthChecks
open Asp.Versioning
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.OpenApi
open Blog.Web.HealthChecks

type Program() = class end


module App =

    open System
    open System.Text
    open Microsoft.AspNetCore.Builder
    open Microsoft.Extensions.DependencyInjection
    open Microsoft.Extensions.Configuration
    open Microsoft.EntityFrameworkCore
    open Microsoft.AspNetCore.Authentication.JwtBearer
    open Microsoft.IdentityModel.Tokens
    open Microsoft.AspNetCore.Http
    open System.Security.Claims
    open Microsoft.Extensions.Hosting

    open Blog.Data
    open Blog.Data.Repository
    open Blog.Data.Models
    open Blog.Services

    // ================= Services =================
    let configureServices (builder: WebApplicationBuilder) =
        let services = builder.Services
        let configuration = builder.Configuration

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)

        //-----------------------------------------
        // Options
        //-----------------------------------------

        services.AddOptions() |> ignore

        services.AddLocalization() |> ignore

        //-----------------------------------------
        // Memory Cache
        //-----------------------------------------

        services.AddMemoryCache() |> ignore

        services.AddResponseCaching() |> ignore

        services.AddDistributedMemoryCache() |> ignore

        //-----------------------------------------
        // AutoMapper
        //-----------------------------------------

        services.AddAutoMapper(typeof<Program>)
        |> ignore

        //-----------------------------------------
        // Controllers
        //-----------------------------------------

        services.AddControllers()
        |> ignore

        services.AddEndpointsApiExplorer()
        |> ignore

        //-----------------------------------------
        // EF Core
        //-----------------------------------------

        services.AddDbContextPool<ApplicationDbContext>(
            fun options ->

                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")
                )
                |> ignore
        )
        |> ignore

        //-----------------------------------------
        // Configuration
        //-----------------------------------------

        services.Configure<BlogConfiguration>(
            configuration.GetSection(nameof(BlogConfiguration))
        )
        |> ignore

        services.Configure<SmtpOptions>(
            configuration.GetSection(nameof(SmtpOptions))
        )
        |> ignore

        services.Configure<EmailExtensionOptions>(
            configuration.GetSection(nameof(EmailExtensionOptions))
        )
        |> ignore

        services.Configure<RedisCacheConfiguration>(
            configuration.GetSection(nameof(RedisCacheConfiguration))
        )
        |> ignore

        services.AddTransient<BlogConfiguration>(fun sp ->

            sp.GetRequiredService<IOptions<BlogConfiguration>>().Value

        ) |> ignore

        services.AddTransient<HostingConfig>()
        |> ignore

        //-----------------------------------------
        // Identity
        //-----------------------------------------

        services.AddScoped<RoleManager<ApplicationRole>>()
            |> ignore

        let identity =
            services.AddIdentityCore<ApplicationUser>(fun options ->

            options.Password.RequireDigit <- false
            options.Password.RequireLowercase <- false
            options.Password.RequireUppercase <- false
            options.Password.RequireNonAlphanumeric <- false
            options.Password.RequiredLength <- 6

        )

        let identityBuilder =
            IdentityBuilder(
                identity.UserType,
                typeof<ApplicationRole>,
                services
            )

        identityBuilder
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserStore<ApplicationUserStore>()
            .AddRoleStore<ApplicationRoleStore>()
            .AddDefaultTokenProviders()
            |> ignore

        //-----------------------------------------
        // JWT
        //-----------------------------------------

        let jwtSection =
            configuration.GetSection(nameof(JwtIssuerOptions))

        let secretKey =
            "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"

        let signingKey =
            SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(secretKey)
            )

        services.AddTransient<IJwtFactory, JwtFactory>()
        |> ignore

        builder.Services.Configure<JwtIssuerOptions>(
            fun (options: JwtIssuerOptions) ->
                options.Issuer <- jwtSection["Issuer"]
                options.Audience <- jwtSection["Audience"]
                options.SigningCredentials <-
                    SigningCredentials(
                        signingKey,
                        SecurityAlgorithms.HmacSha256
                    )
        )
        |> ignore

        let issuer =
            jwtSection.["Issuer"]

        let audience =
            jwtSection.["Audience"]

        let validation =
            TokenValidationParameters(
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                RequireExpirationTime = false,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = signingKey
            )

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(fun options ->

                options.ClaimsIssuer <- issuer
                options.TokenValidationParameters <- validation
                options.SaveToken <- true

            )
        |> ignore

        services.Configure<JwtIssuerOptions>(fun (options: JwtIssuerOptions) ->
            options.Issuer <- issuer
            options.Audience <- audience

            options.SigningCredentials <-
                SigningCredentials(
                    signingKey,
                    SecurityAlgorithms.HmacSha256
                )
        ) |> ignore

        //-----------------------------------------
        // Authorization
        //-----------------------------------------

        services.AddAuthorization(fun options ->

            options.AddPolicy(
                "ApiUser",
                fun policy ->
                    policy.RequireClaim(
                        JwtClaimTypes.Rol,
                        JwtClaims.ApiAccess
                    )
                    |> ignore
            )

        )
            |> ignore

        services.AddSingleton<IAuthorizationHandler, HasScopeHandler>()
            |> ignore

        //-----------------------------------------
        // HttpContext
        //-----------------------------------------

        services.AddHttpContextAccessor()
            |> ignore

        //-----------------------------------------
        // Email
        //-----------------------------------------

        services.AddSingleton<IEmailService, SmtpEmailService>()
            |> ignore

        services.AddTransient<IEmailExtensionService, EmailExtensionService>()
            |> ignore

        services.AddTransient<IEmailTemplateProvider, SimpleEmailTemplateProvider>()
            |> ignore

        //-----------------------------------------
        // Cache
        //-----------------------------------------

        services.AddTransient<IStaticCacheManager, MemoryCacheManager>()
            |> ignore

        services.AddTransient<IResponseCacheService, ResponseCacheService>()
            |> ignore

        //-----------------------------------------
        // Helpers
        //-----------------------------------------

        services.AddTransient<IWebHelper, WebHelper>()
            |> ignore

        services.AddTransient<IShareFileProvider, FileProvider>()
            |> ignore

        services.AddTransient<IControllerContext, MyControllerContext>()
            |> ignore

            //-----------------------------------------
        // Repositories
        //-----------------------------------------

        services.AddScoped<IRepository<ApplicationUser>, Repository<ApplicationUser>>()
        |> ignore

        services.AddScoped<IRepository<RefreshToken>, Repository<RefreshToken>>()
        |> ignore

        services.AddScoped<IRepository<Post>, Repository<Post>>()
        |> ignore

        services.AddScoped<IRepository<Comment>, Repository<Comment>>()
        |> ignore

        services.AddScoped<IRepository<Message>, Repository<Message>>()
        |> ignore

        services.AddScoped<IRepository<Profile>, Repository<Profile>>()
        |> ignore

        services.AddScoped<IRepository<Tag>, Repository<Tag>>()
        |> ignore

        services.AddScoped<IRepository<PostsTagsRelations>, Repository<PostsTagsRelations>>()
        |> ignore

        services.AddScoped<IRepository<Setting>, Repository<Setting>>()
        |> ignore

        //-----------------------------------------
        // Domain Services
        //-----------------------------------------

        services.AddTransient<IAuthService, AuthService>()
        |> ignore

        services.AddTransient<IUserService, UserService>()
        |> ignore

        services.AddTransient<IRegistrationService, RegistrationService>()
        |> ignore

        services.AddTransient<IRefreshTokenService, RefreshTokenService>()
        |> ignore

        services.AddTransient<IPostsService, PostsService>()
        |> ignore

        services.AddTransient<ICommentsService, CommentsService>()
        |> ignore

        services.AddTransient<IProfileService, ProfileService>()
        |> ignore

        services.AddTransient<IMessagesService, MessagesService>()
        |> ignore

        services.AddTransient<ITagsService, TagsService>()
        |> ignore

        services.AddTransient<IPostsTagsRelationsService, PostsTagsRelationsService>()
        |> ignore

        //-----------------------------------------
        // MediatR
        //-----------------------------------------

        let mediatr =
            MediatRServiceConfiguration()

        mediatr.RegisterServicesFromAssembly(typeof<Program>.Assembly)

        services.AddMediatR(mediatr)
        |> ignore

        //-----------------------------------------
        // Health Checks
        //-----------------------------------------

        services
            .AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>()
            |> ignore

        services.Configure<HealthCheckPublisherOptions>(fun (options: HealthCheckPublisherOptions) ->

            options.Delay <- TimeSpan.FromSeconds(2.0)

            options.Predicate <-
                Func<HealthCheckRegistration, bool>(fun check ->
                    check.Tags.Contains("ready")
                )
        )
        |> ignore

        services.AddSingleton<IHealthCheckPublisher, ReadinessPublisher>()
        |> ignore

        //-----------------------------------------
        // Redis
        //-----------------------------------------

        let redis =
            RedisCacheConfiguration()

        configuration
            .GetSection(nameof(RedisCacheConfiguration))
            .Bind(redis)

        services.AddSingleton(redis)
        |> ignore

        if redis.Enabled then

            services.AddStackExchangeRedisCache(fun options ->

                options.Configuration <- redis.ConnectionString

            )
            |> ignore

        //-----------------------------------------
        // CORS
        //-----------------------------------------

        services.AddCors(fun options ->

            options.AddPolicy(
                "CorsPolicy",
                fun builder ->

                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                    |> ignore

            )

        )
        |> ignore

        //-----------------------------------------
        // API Versioning
        //-----------------------------------------

        services
            .AddApiVersioning(fun options ->

                options.ReportApiVersions <- true

                options.DefaultApiVersion <- ApiVersion(1,0)

                options.AssumeDefaultVersionWhenUnspecified <- true

            )
            .AddApiExplorer(fun options ->

                options.GroupNameFormat <- "'v'VVV"

                options.SubstituteApiVersionInUrl <- true

            )
        |> ignore

        //-----------------------------------------
        // Swagger
        //-----------------------------------------

        services.AddSwaggerGen(fun c ->

            c.SwaggerDoc(
                "v1",
                OpenApiInfo(
                    Title = "Blog API",
                    Version = "v1",
                    Description = "Blog REST API"
                )
            )

            c.ResolveConflictingActions(fun api ->
                Seq.head api
            )

        )
        |> ignore

        // ================= ENTRY POINT =================


        app.Run()

        0