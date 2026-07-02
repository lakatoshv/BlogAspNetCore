namespace Blog.FSharpRESTAPI.Cache

open System
open System.Text
open System.Threading.Tasks

open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc.Filters
open Microsoft.Extensions.DependencyInjection

open Blog.Core.Configuration
open Blog.CommonServices.Interfaces

[<AttributeUsage(AttributeTargets.Class ||| AttributeTargets.Method)>]
type CachedAttribute(lifeTimeSeconds: int) =
    inherit Attribute()

    static member private GenerateCacheKeyFromRequest(request: HttpRequest) =

        let builder = StringBuilder()

        builder.Append(request.Path.ToString()) |> ignore

        request.Query
        |> Seq.sortBy (fun q -> q.Key)
        |> Seq.iter (fun kv ->
            builder.Append($"{kv.Key}-{kv.Value}") |> ignore
        )

        builder.ToString()

    interface IAsyncActionFilter with

        member _.OnActionExecutionAsync
            (
                context: ActionExecutingContext,
                next: ActionExecutionDelegate
            ) : Task =

            task {

                let cacheSettings =
                    context.HttpContext.RequestServices
                        .GetRequiredService<RedisCacheConfiguration>()

                if not cacheSettings.Enabled then

                    // execute controller immediately
                    let! _ = next.Invoke()

                    return ()

                else

                    let cacheService =
                        context.HttpContext.RequestServices
                            .GetRequiredService<IResponseCacheService>()

                    let cacheKey =
                        CachedAttribute.GenerateCacheKeyFromRequest(
                            context.HttpContext.Request
                        )

                    let! cachedResponse =
                        cacheService.GetCachedResponseAsync(cacheKey)

                    if not (String.IsNullOrWhiteSpace(cachedResponse)) then

                        let result = ContentResult()

                        result.Content <- cachedResponse
                        result.ContentType <- "application/json"
                        result.StatusCode <- Nullable 200

                        context.Result <- result

                        return ()

                    else

                        let! executedContext =
                            next.Invoke()

                        match executedContext.Result with

                        | :? OkObjectResult as okResult ->

                            do!
                                cacheService.CacheResponseAsync(
                                    cacheKey,
                                    okResult.Value,
                                    TimeSpan.FromSeconds(float lifeTimeSeconds)
                                )

                        | _ -> ()

                        return ()
            }
            :> Task