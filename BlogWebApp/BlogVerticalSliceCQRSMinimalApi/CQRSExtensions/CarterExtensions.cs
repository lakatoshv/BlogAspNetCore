using Asp.Versioning.Builder;
using Carter;

namespace BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

public static class CarterExtensions
{
    public static IEndpointRouteBuilder MapVersionedCarter(
        this IEndpointRouteBuilder app,
        ApiVersionSet versionSet)
    {
        var group = app.MapGroup("/api/v{version:apiVersion}")
            .WithApiVersionSet(versionSet);

        group.MapCarter(); // 👈 ключ

        return app;
    }
}