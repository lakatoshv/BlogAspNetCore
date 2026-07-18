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

    [<HttpPost(ApiRoutes.TagsController.GetTagsByFilter)>]
    [<ProducesResponseType(typeof<PagedTagsResponse>, 200)>]
    [<ProducesResponseType(404)>]
    [<Cached(600)>]
    member this.GetTagsByFilter
        (
            [<FromBody>] searchParameters : SearchParametersRequest
        ) : Task<IActionResult> =

        task {

            if isNull searchParameters then
                return this.BadRequest() :> IActionResult
            else

                if isNull searchParameters.SortParameters then
                    searchParameters.SortParameters <- SortParametersRequest()

                if String.IsNullOrWhiteSpace(searchParameters.SortParameters.OrderBy) then
                    searchParameters.SortParameters.OrderBy <- "asc"

                if String.IsNullOrWhiteSpace(searchParameters.SortParameters.SortBy) then
                    searchParameters.SortParameters.SortBy <- "Title"

                if not searchParameters.SortParameters.CurrentPage.HasValue then
                    searchParameters.SortParameters.CurrentPage <- Nullable 1

                if not searchParameters.SortParameters.PageSize.HasValue then
                    searchParameters.SortParameters.PageSize <- Nullable 10

                let! tags =
                    mapper.Map<SearchParametersDto>(searchParameters)
                    |> tagsService.GetTagsAsync

                if isNull tags then
                    return this.NotFound() :> IActionResult
                else
                    return
                        mapper.Map<PagedTagsResponse>(tags)
                        |> this.Ok
                        :> IActionResult
        }

    [<HttpGet(ApiRoutes.TagsController.GetAvailableTags)>]
    [<ProducesResponseType(typeof<ResizeArray<TagResponse>>, 200)>]
    [<ProducesResponseType(404)>]
    [<Cached(600)>]
    member this.GetAvailableTags([<FromRoute>] postId : int) : Task<IActionResult> =
        task {

            let spec =
                TagSpecification(fun x ->
                    isNull x.PostsTagsRelations ||
                    x.PostsTagsRelations.Any(fun y -> y.PostId <> postId))

            let! tags = tagsService.GetAllAsync(spec)

            if isNull tags then
                return this.NotFound() :> IActionResult
            else
                return
                    mapper.Map<ResizeArray<TagResponse>>(tags)
                    |> this.Ok
                    :> IActionResult
        }

    [<HttpGet(ApiRoutes.TagsController.GetTag)>]
    [<ProducesResponseType(typeof<TagResponse>, 200)>]
    [<ProducesResponseType(404)>]
    [<Cached(600)>]
    member this.GetTag([<FromRoute>] id : int) : Task<IActionResult> =
        task {

            let! tag = tagsService.FindAsync(id)

            if isNull tag then
                return this.NotFound() :> IActionResult
            else
                return
                    mapper.Map<TagResponse>(tag)
                    |> this.Ok
                    :> IActionResult
        }

    [<HttpGet(ApiRoutes.TagsController.TagsActivity)>]
    [<ProducesResponseType(typeof<ChartDataModel>, 200)>]
    [<ProducesResponseType(404)>]
    [<Cached(600)>]
    member this.TagsActivity() : Task<IActionResult> =
        task {

            let! activity =
                tagsService.GetTagsActivity()

            if isNull activity then
                return this.NotFound() :> IActionResult
            else
                return this.Ok(activity) :> IActionResult
        }