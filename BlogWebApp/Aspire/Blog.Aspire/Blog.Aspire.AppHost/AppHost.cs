var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiServices = builder.AddProject<Projects.Blog_Web>("BlogWeb");
//.WithHttpHealthCheck("/health");



builder.Build().Run();
