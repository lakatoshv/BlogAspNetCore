namespace Blog.FSharpRESTAPI.Controllers

open Blog.Data.Models;
open Microsoft.AspNetCore.Authorization;
open Microsoft.AspNetCore.Identity;
open Microsoft.AspNetCore.Mvc;
open Microsoft.EntityFrameworkCore;
open System;
open System.Threading.Tasks;
open System.Collections.Generic;
open Microsoft.AspNetCore.Mvc.ModelBinding;
open AutoMapper;
open Blog.Services.Core.Utilities;
open Blog.CommonServices.EmailServices.Interfaces;
open Blog.EntityServices.ControllerContext;
open Blog.Contracts.V1;
open Blog.Contracts.V1.Requests.UsersRequests;
open Blog.Contracts.V1.Responses.UsersResponses;
open Blog.Contracts.V1.Responses.Chart;
open Blog.Core.Consts;
open Blog.EntityServices.EntityFrameworkServices.Identity.Auth;
open Blog.EntityServices.EntityFrameworkServices.Identity.Registration;
open Blog.EntityServices.EntityFrameworkServices.Identity.User;
open Blog.FSharpRESTAPI.Cache

[<Route(ApiRoutes.AccountsController.Accounts)>]
[<ApiController>]
[<Authorize>]
[<Produces(Consts.JsonType)>]
type AccountsController
    (
        controllerContext : IControllerContext,
        registrationService : IRegistrationService,
        authService : IAuthService,
        userManager : UserManager<ApplicationUser>,
        roleManager : RoleManager<ApplicationRole>,
        userService : IUserService,
        emailService : IEmailExtensionService,
        mapper : IMapper
    ) =
    inherit BaseController(controllerContext)

    [<HttpGet(ApiRoutes.AccountsController.Initialize)>]
    [<ProducesResponseType(typeof<List<RoleResponse>>, 200)>]
    [<ProducesResponseType(typeof<string>, 400)>]
    [<AllowAnonymous>]
    member this.Initialize([<FromRoute>] userId : string) : Task<IActionResult> =
        task {

            if String.IsNullOrWhiteSpace(userId) then
                return this.BadError("Something Went Wrong")
            else
                let! roles = roleManager.Roles.ToListAsync()

                return
                    (this.Ok(
                        mapper.Map<List<RoleResponse>>(roles)
                    ) :> IActionResult)
        }

    [<HttpGet(ApiRoutes.AccountsController.GetAllUsers)>]
    [<AllowAnonymous>]
    [<ProducesResponseType(typeof<List<AccountResponse>>, 200)>]
    member this.GetAllUsers() =
        task {
            let! users =
                userManager.Users
                    .Include(fun u -> u.Roles)
                    .ToListAsync()

            return
                users
                |> mapper.Map<List<AccountResponse>>
                |> this.Ok
        }

    [<HttpGet(ApiRoutes.AccountsController.SendConfirmationEmail)>]
    [<ProducesResponseType(204)>]
    member this.SendVerificationEmailAsync() =
        task {
            let! token =
                userService.GetEmailVerificationTokenAsync(this.CurrentUser.Email)

            do!
                emailService.SendVerificationEmailAsync(this.UserName, token)

            return this.NoContent()
        }

    [<HttpGet(ApiRoutes.AccountsController.UsersActivity)>]
    [<ProducesResponseType(typeof<ChartDataModel>, 200)>]
    [<ProducesResponseType(typeof<string>, 400)>]
    [<Cached(600)>]
    member this.GetUsersActivity() : Task<IActionResult> =
        task {

            let! activity =
                userService.GetUsersActivity()

            if isNull activity then
                return (this.NotFound() :> IActionResult)
            else
                return (this.Ok(activity) :> IActionResult)
        }