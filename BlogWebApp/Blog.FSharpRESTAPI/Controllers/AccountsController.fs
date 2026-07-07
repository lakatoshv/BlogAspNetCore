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