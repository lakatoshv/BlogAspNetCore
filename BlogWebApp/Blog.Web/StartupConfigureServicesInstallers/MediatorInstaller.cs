namespace Blog.Web.StartupConfigureServicesInstallers
{
    using Blog.Core.Infrastructure.Mediator.Behaviors;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Mediator installer.
    /// </summary>
    /// <seealso cref="IInstaller" />
    public class MediatorInstaller : IInstaller
    {
        /// <inheritdoc cref="IInstaller"/>
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        }
    }
}