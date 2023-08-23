namespace Blog.Web.StartupConfigureServicesInstallers;

using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Blog.Services.Core.Identity.Auth;

/// <summary>
/// Authentication and authorization installer.
/// </summary>
/// <seealso cref="IInstaller" />
public class AuthenticationAndAuthorizationInstaller : IInstaller
{
    /// <inheritdoc cref="IInstaller"/>
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        // TODO: Extract to external extension method .AddJWT()

        // Get options from app settings            
        var jwtAppSettingOptions = configuration.GetSection(nameof(JwtIssuerOptions));

        // TODO: Get this from somewhere secure, possibly configuration !!!
        // TODO: Implement some kind of key vault service to store Secret
        const string secretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH";
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
            options.AddPolicy("AllowAll", bilder =>
            {
                bilder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
            options.AddPolicy("AllowAllBlazor", bilder =>
            {
                bilder.WithOrigins("https://localhost:44390").AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }
}