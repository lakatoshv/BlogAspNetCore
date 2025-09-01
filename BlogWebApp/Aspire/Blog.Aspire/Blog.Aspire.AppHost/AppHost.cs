var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");



builder.Build().Run();
