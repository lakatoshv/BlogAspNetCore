using Blog.Web.StartupConfigures;
using Blog.Web.StartupConfigureServicesInstallers;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.InstallServicesInAssembly(builder.Configuration);

var app = builder.Build();

ConfigureBase.Configure(app, builder.Environment);
ConfigureSwagger.Configure(app, builder.Configuration);
ConfigureAuthentication.Configure(app);
ConfigureRoutes.Configure(app);
ConfigureSpa.Configure(app, builder.Environment);

app.MapControllers();

app.Run();

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program { }
#pragma warning restore CA1050 // Declare types in namespaces
