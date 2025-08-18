namespace Blog.FSharp.Core.Infrastructure.Middlewares

open Microsoft.AspNetCore.Builder

[<AutoOpen>]
module ApplicationBuilderExtensions =

    type IApplicationBuilder with
        /// <summary>
        /// Uses the e tagger.
        /// </summary>
        member this.UseETagger() =
            this.UseMiddleware<ETagMiddleware>() |> ignore