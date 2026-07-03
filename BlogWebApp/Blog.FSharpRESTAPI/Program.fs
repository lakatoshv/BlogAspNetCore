open Blog.Core.Configuration
open Blog.Services.Core.Caching.Interfaces
open Blog.Services.Core.Caching
open Blog.Core.Interfaces
open Blog.Core
open Blog.Core.Infrastructure
open Blog.EntityServices.ControllerContext
open Microsoft.AspNetCore.Authorization
open Blog.Services.Core.Security
open Microsoft.AspNetCore.Identity
open Blog.EntityServices.EntityFrameworkServices.Identity.Auth
open Blog.EntityServices.EntityFrameworkServices.Identity.User
open Blog.CommonServices.EmailServices.Interfaces
open Blog.EntityServices.EntityFrameworkServices.Identity.RefreshToken
open Blog.EntityServices.EntityFrameworkServices.Identity.Registration
open Blog.Services.Core.Email.Templates
open Blog.EntityServices.Interfaces
open Blog.EntityServices.EntityFrameworkServices
open Blog.CommonServices.Interfaces
open Blog.CommonServices
open Blog.CommonServices.EmailServices
open Microsoft.Extensions.Options
open Blog.Services.Core.Email.Smtp
open Blog.Core.Emails
open Blog.Services.Core.Identity.Auth
open Microsoft.Extensions.Diagnostics.HealthChecks
open Asp.Versioning
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.OpenApi
open Blog.Web.HealthChecks

type Program() = class end


module App =

    open System
    open System.Text
    open Microsoft.AspNetCore.Builder
    open Microsoft.Extensions.DependencyInjection
    open Microsoft.Extensions.Configuration
    open Microsoft.EntityFrameworkCore
    open Microsoft.AspNetCore.Authentication.JwtBearer
    open Microsoft.IdentityModel.Tokens
    open Microsoft.AspNetCore.Http
    open System.Security.Claims
    open Microsoft.Extensions.Hosting

    open Blog.Data
    open Blog.Data.Repository
    open Blog.Data.Models
    open Blog.Services

    // ================= Services =================
    let configureServices (builder: WebApplicationBuilder) =
        let services = builder.Services
        let configuration = builder.Configuration

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)

        //-----------------------------------------
        // Options
        //-----------------------------------------

        services.AddOptions() |> ignore

        services.AddLocalization() |> ignore

        //-----------------------------------------
        // Memory Cache
        //-----------------------------------------

        services.AddMemoryCache() |> ignore

        services.AddResponseCaching() |> ignore

        services.AddDistributedMemoryCache() |> ignore

        //-----------------------------------------
        // AutoMapper
        //-----------------------------------------

        services.AddAutoMapper(typeof<Program>)
        |> ignore

        //-----------------------------------------
        // Controllers
        //-----------------------------------------

        services.AddControllers()
        |> ignore

        services.AddEndpointsApiExplorer()
        |> ignore

        app.Run()

        0