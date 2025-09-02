var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiServices = builder.AddProject<Projects.Blog_Web>("BlogWeb");
//.WithHttpHealthCheck("/health");

builder.AddProject<Projects.BlogRazor>("BlogRazor")
    //.WithLaunchProfile("ShopRazor")
    .WithExternalHttpEndpoints();
//.WithHttpHealthCheck("/health");
//.WithReference(cache)
//.WaitFor(cache);
//.WithReference(apiService)
//.WaitFor(apiService);

builder.Build().Run();
