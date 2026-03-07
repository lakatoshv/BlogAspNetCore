using BlogMinimalApi.ApiEndpoints;
using BlogMinimalApi.StartupConfigures;
using BlogMinimalApi.StartupConfigureServicesInstallers;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.InstallServicesInAssembly(builder.Configuration);

var app = builder.Build();

ConfigureBase.Configure(app, builder.Environment);
ConfigureSwagger.Configure(app, builder.Configuration);
ConfigureAuthentication.Configure(app);
ConfigureRoutes.Configure(app);

//Api endpoints
app.InstallApiRoutesInAssembly();

app.Run();