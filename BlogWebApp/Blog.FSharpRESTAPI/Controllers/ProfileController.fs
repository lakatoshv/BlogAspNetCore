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

    [<HttpPut("{profileId:int}")>]
    [<Authorize>]
    [<ProducesResponseType(typeof<ApplicationUserResponse>, 204)>]
    [<ProducesResponseType(typeof<obj>, 400)>]
    [<ProducesResponseType(404)>]
    member this.EditAsync
        (
            [<FromRoute>] profileId : int,
            [<FromBody>] model : UpdateProfileRequest
        ) : Task<IActionResult> =

        task {

            if isNull this.CurrentUser then
                return this.BadError("Unauthorized")

            else

                let! profile =
                    profileService.GetProfile(profileId)

                if isNull profile then
                    return (this.NotFound() :> IActionResult)

                else

                    if not (profile.Profile.UserId.Equals(this.CurrentUser.Id)) then
                        return this.BadError("You are not allowed to edit profile.")

                    else

                        profile.Profile.User.Email <- model.Email
                        profile.Profile.User.FirstName <- model.FirstName
                        profile.Profile.User.LastName <- model.LastName
                        profile.Profile.User.PhoneNumber <- model.PhoneNumber
                        profile.Profile.About <- model.About

                        do!
                            profileService.UpdateAsync(profile.Profile)

                        let! editedProfile =
                            profileService.GetProfile(profileId)

                        let response =
                            mapper.Map<ApplicationUserResponse>(editedProfile)

                        return
                            this.Ok(response)
                            :> IActionResult
        }