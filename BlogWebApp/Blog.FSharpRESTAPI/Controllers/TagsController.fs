namespace Blog.FSharpRESTAPI.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks

open AutoMapper

open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc.ModelBinding


open Blog.Contracts.V1
open Blog.Contracts.V1.Requests.TagsRequests
open Blog.Contracts.V1.Requests
open Blog.Contracts.V1.Responses
open Blog.Contracts.V1.Responses.TagsResponses

open Blog.Data.Models
open Blog.Data.Specifications

open Blog.EntityServices.ControllerContext
open Blog.EntityServices.Interfaces

open Blog.Core.Consts
open Blog.FSharpRESTAPI.Cache
open Blog.Services.Core.Dtos
open Blog.Contracts.V1.Responses.Chart

[<Route(ApiRoutes.TagsController.Tags)>]
[<ApiController>]
[<Produces(Consts.JsonType)>]
type TagsController(
    controllerContext : IControllerContext,
    tagsService : ITagsService,
    mapper : IMapper
) =
    inherit BaseController(controllerContext)

    [<HttpGet(ApiRoutes.TagsController.GetTags)>]
    [<ProducesResponseType(typeof<ResizeArray<TagResponse>>, 200)>]
    [<ProducesResponseType(404)>]
    [<Cached(600)>]
    member this.GetTags() : Task<IActionResult> =
        task {

            let! tags = tagsService.GetAllAsync()

            if isNull tags then
                return this.NotFound() :> IActionResult
            else
                let response =
                    mapper.Map<ResizeArray<TagResponse>>(tags)

                return this.Ok(response) :> IActionResult
        }