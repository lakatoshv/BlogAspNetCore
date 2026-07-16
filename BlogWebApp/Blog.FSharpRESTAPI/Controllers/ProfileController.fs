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

    [<ProducesResponseType(typeof<ApplicationUserResponse>, 200)>]
    [<ProducesResponseType(404)>]
    [<HttpGet("{id:int}")>]
    [<Cached(600)>]
    member this.Show([<FromRoute>] id : int) : Task<IActionResult> =
        task {

            let! profile =
                profileService.GetProfile(id)

            if isNull profile then
                return (this.NotFound() :> IActionResult)

            else

                profile.Profile.User <- null

                let response =
                    mapper.Map<ApplicationUserResponse>(profile)

                return
                    this.Ok(response)
                    :> IActionResult
        }