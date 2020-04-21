using Blog.Core.Emails;
using Blog.Data;
using Blog.Data.Models;
using Blog.Data.Repository;
using Blog.Services.Core.Email.Smtp;
using Blog.Services.Core.Email.Templates;
using Blog.Services.Core.Identity.Auth;
using Blog.Services.EmailServices;
using Blog.Services.EmailServices.Interfaces;
using Blog.Services.Identity.Auth;
using Blog.Services.Identity.RefreshToken;
using Blog.Services.Identity.Registration;
using Blog.Services.Identity.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Configuration;
using Microsoft.Extensions.Options;
using Blog.Services.Core.Caching.Interfaces;
using Blog.Services.Core.Caching;
using Blog.Core.Interfaces;
using Blog.Core;
using Blog.Core.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Blog.Services.Core.Security;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Rewrite;
using Blog.Services.Interfaces;
using Blog.Services;
using Blog.Services.ControllerContext;

namespace Blog.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<ApplicationDbContext>(
                options => options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));

            /*
            services.AddIdentity<ApplicationUser, ApplicationRole>(
                options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserStore<ApplicationUserStore>()
                .AddRoleStore<ApplicationRoleStore>()
                .AddDefaultTokenProviders();
            */

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            #region JWT
            // TODO: Extract to external extension method .AddJWT()

            // Get options from app settings            
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            // TODO: Get this from somewhere secure, possibly configuration !!!
            // TODO: Implement some kind of keyvault service to store Secret
            var secretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH";
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            // JWT wire up
            services.AddTransient<IJwtFactory, JwtFactory>();

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            //  Configure JWT auth
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
                /*
                configureOptions.Events = new JwtBearerEvents
                {

                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    },

                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/hubs/notification") || path.StartsWithSegments("/hubs/message")))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };*/
            });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(JwtClaimTypes.Rol, JwtClaims.ApiAccess));
            });
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", bilder =>
                {
                    bilder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            #endregion

            // Add Identity 
            // TODO: Extract to external extension method .AddIdentity()
            services.AddScoped<RoleManager<ApplicationRole>>();

            var builder = services.AddIdentityCore<ApplicationUser>(o =>
            {
                // Configure Identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });


            builder = new IdentityBuilder(builder.UserType, typeof(ApplicationRole), builder.Services);

            builder.AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            #region Email

            // TODO: Extract to external extension method .AddEmail()
            services.AddSingleton<IEmailService, SmtpEmailService>();

            // Configure SmtpOptions
            services.Configure<SmtpOptions>(options =>
            {
                options.Host = Configuration["Host"];
                options.Port = int.Parse(Configuration["Port"]);
                options.UserName = Configuration["EmailUserName"];
                options.Password = Configuration["Password"];
                //options.EnableSsl = bool.Parse(Configuration["EnableSsl"]);
            });

            // Configure SystemEmailOptions
            services.Configure<EmailExtensionOptions>(options =>
            {
                options.BaseUrl = Configuration["BaseUrl"];
                options.From = Configuration["From"];
            });

            #endregion

            services.AddSingleton(this.Configuration);

            // Data repositories
            //services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            // services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            // Application services

            // Identity stores
            services.AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, ApplicationRoleStore>();
            services.AddTransient<IRepository<ApplicationUser>, TableMethods<ApplicationUser>>();

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IEmailExtensionService, EmailExtensionService>();

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IRefreshTokenService, RefreshTokenService>();
            services.AddTransient<IRegistrationService, RegistrationService>();
            services.AddTransient<IEmailTemplateProvider, SimpleEmailTemplateProvider>();

            services.AddTransient<IPostsService, PostsService>();

            services.AddTransient<IRepository<RefreshToken>, TableMethods<RefreshToken>>();
            services.AddTransient<IRepository<Setting>, TableMethods<Setting>>();
            services.AddTransient<IRepository<Post>, TableMethods<Post>>();

            services.AddTransient(x => x.GetService<IOptions<BlogConfiguration>>().Value);
            services.AddTransient<HostingConfig>();

            services.AddTransient<IStaticCacheManager, MemoryCacheManager>();
            //services.AddTransient<IDbContext, ApplicationDbContext>();
            services.AddTransient<IWebHelper, WebHelper>();
            services.AddTransient<IShareFileProvider, FileProvider>();

            services.AddTransient<IControllerContext, MyControllerContext>();
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
            services.AddHttpContextAccessor();


            // register the scope authorization handler
            services.AddAutoMapper();
            services.AddMvc();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // TODO: Add development configuration
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseAuthentication();

            // TODO: Implement more advanced Error Handling
            // TODO: Extract implementations to external files
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        //context.Response.AddApplicationError(error.Error.Message);
                        await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                    }
                });
            });


            // Redirect any non-API calls to the Angular application
            // so our application can handle the routing
            // TODO: Extract to extension method .AddAngular()
            app.Use(async (context, next) =>
            {
                await next();

                if (
                    context.Response.StatusCode == 404 &&
                    !Path.HasExtension(context.Request.Path.Value) &&
                    !context.Request.Path.Value.StartsWith("/api/")
                    )
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });

            // Configure application for usage as API
            // with default route of '/api/[Controller]'

            app.UseMvcWithDefaultRoute();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Configures application to serve the index.html file from /wwwroot
            // when you access the server from a browser
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseCors("EnableCORS");

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
                else
                {
                    RedirectFromHttpToHttps(app);
                }
            });
        }

        private void RedirectFromHttpToHttps(IApplicationBuilder applicationBuilder)
        {
            var options = new RewriteOptions().AddRedirectToHttps();
            applicationBuilder.UseRewriter(options);
        }
    }
}
