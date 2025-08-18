namespace Blog.FSharp.Core.Infrastructure.Middlewares

open System
open System.Threading.Tasks
open Microsoft.AspNetCore.Http
open Newtonsoft.Json
open Blog.FSharp.Core.Helpers

type ErrorHandlingMiddleware(next: RequestDelegate) =

    let _next = next

    member _.Invoke(context: HttpContext) : Task =
        task {
            try
                do! _next.Invoke(context)
            with ex ->
                do! ErrorHandlingMiddleware.HandleExceptionAsync(context, ex)
        }

    static member private HandleExceptionAsync(context: HttpContext, ex: Exception) : Task =
        task {
            try
                let result = JsonConvert.SerializeObject(ExceptionHelper.getMessages(ex), Formatting.Indented)
                if result.Length > 4000 then
                    do! context.Response.WriteAsync("Error message too long. Please use DEBUG in HandleExceptionAsync to see full exception.")
                else
                    do! context.Response.WriteAsync(result)
            with _ ->
                do! context.Response.WriteAsync(sprintf "%s For more info use DEBUG in HandleExceptionAsync." ex.Message)
        }