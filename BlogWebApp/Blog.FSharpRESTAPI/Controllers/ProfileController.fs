namespace Blog.FSharpRESTAPI.Controllers

open System.Threading.Tasks

open AutoMapper

open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc

open Blog.Contracts.V1
open Blog.Contracts.V1.Requests.UsersRequests
open Blog.Contracts.V1.Responses.UsersResponses
open Blog.Core.Consts

open Blog.EntityServices.ControllerContext
open Blog.EntityServices.Interfaces

open Blog.FSharpRESTAPI.Cache

[<Route(ApiRoutes.ProfileController.Profile)>]
[<ApiController>]
[<Produces(Consts.JsonType)>]
type ProfileController(
        controllerContext : IControllerContext,
        profileService : IProfileService,
        mapper : IMapper
    ) =
    inherit BaseController(controllerContext)
