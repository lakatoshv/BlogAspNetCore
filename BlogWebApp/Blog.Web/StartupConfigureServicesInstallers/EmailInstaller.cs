using Blog.CommonServices.EmailServices;
using Blog.CommonServices.EmailServices.Interfaces;
using Blog.Core.Emails;
using Blog.Services.Core.Email.Smtp;
using Blog.Services.Core.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Web.StartupConfigureServicesInstallers
{
    /// <summary>
    /// Email installer.
    /// </summary>
    /// <seealso cref="IInstaller" />
    public class EmailInstaller : IInstaller
    {
        /// <inheritdoc cref="IInstaller"/>
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            // TODO: Extract to external extension method .AddEmail()
            services.AddSingleton<IEmailService, SmtpEmailService>();

            // Configure SmtpOptions
            var smtpOptions = configuration.GetSection(nameof(SmtpOptions));

            // Configure SmtpOptions
            services.Configure<SmtpOptions>(options =>
            {
                options.Host = smtpOptions[nameof(SmtpOptions.Host)];
                options.Port = smtpOptions[nameof(SmtpOptions.Port)].ToInt();
                options.UserName = smtpOptions[nameof(SmtpOptions.UserName)];
                options.Password = smtpOptions[nameof(SmtpOptions.Password)];
                options.EnableSsl = smtpOptions[nameof(SmtpOptions.EnableSsl)].ToBool();
            });

            // Configure SystemEmailOptions
            var systemEmailOptions = configuration.GetSection(nameof(EmailExtensionOptions));
            services.Configure<EmailExtensionOptions>(options =>
            {
                options.BaseUrl = systemEmailOptions[nameof(EmailExtensionOptions.BaseUrl)];
                options.From = systemEmailOptions[nameof(EmailExtensionOptions.From)];
            });
        }
    }
}