using Blog.Web.StartupConfigures;
using Blog.Web.StartupConfigureServicesInstallers;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InstallServicesInAssembly(builder.Configuration);

var app = builder.Build();

ConfigureBase.Configure(app, builder.Environment);
ConfigureSwagger.Configure(app, builder.Configuration);
ConfigureAuthentication.Configure(app);
ConfigureRoutes.Configure(app);
ConfigureSpa.Configure(app, builder.Environment);

app.Run();

public partial class Program { }