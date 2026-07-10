namespace Blog.FSharpRESTAPI.Controllers

open System
open System.Collections.Generic
open System.Threading.Tasks

open AutoMapper

open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc.ModelBinding
open Microsoft.EntityFrameworkCore

open Blog.Contracts.V1
open Blog.Contracts.V1.Requests
open Blog.Contracts.V1.Requests.CommentsRequests
open Blog.Contracts.V1.Responses
open Blog.Contracts.V1.Responses.Chart
open Blog.Contracts.V1.Responses.CommentsResponses

open Blog.Data.Models
open Blog.Services.Core.Dtos

open Blog.EntityServices.ControllerContext
open Blog.EntityServices.Interfaces

open Blog.Core.Consts
open Blog.FSharpRESTAPI.Cache

[<Route(ApiRoutes.CommentsController.Comments)>]
[<ApiController>]
[<Produces(Consts.JsonType)>]
type CommentsController
    (
        controllerContext : IControllerContext,
        commentsService : ICommentsService,
        mapper : IMapper
    ) =
    inherit BaseController(controllerContext)

    [<ProducesResponseType(typeof<List<CommentResponse>>, 200)>]
    [<HttpGet>]
    [<Cached(600)>]
    [<ProducesResponseType(typeof<List<CommentResponse>>, 200)>]
    member this.GetAllComments() : Task<IActionResult> =
        task {

            let! comments =
                commentsService.GetAllAsync()

            let response =
                mapper.Map<List<CommentResponse>>(comments)

            return this.Ok(response) :> IActionResult
        }

    [<ProducesResponseType(typeof<List<CommentResponse>>, 200)>]
    [<HttpGet>]
    [<Cached(600)>]
    member this.CommentsActivity() : Task<IActionResult> =
        task {

            let! activity =
                commentsService.GetCommentsActivity()

            if isNull activity then
                return (this.NotFound() :> IActionResult)

            else
                return
                    (this.Ok(activity)
                        :> IActionResult)
        }

    [<ProducesResponseType(typeof<PagedCommentsResponse>, 200)>]
    [<ProducesResponseType(404)>]
    [<HttpPost(ApiRoutes.CommentsController.GetCommentsByFilter)>]
    [<Cached(600)>]
    member this.GetComments(
        [<FromBody>] sortParameters : SortParametersRequest
    )
        : Task<IActionResult> =

        task {

            let sortParameters =
                if isNull sortParameters then
                    SortParametersRequest()
                else
                    sortParameters

            if not sortParameters.CurrentPage.HasValue then
                sortParameters.CurrentPage <- Nullable 1

            sortParameters.PageSize <- 10

            let! comments =
                commentsService.GetPagedComments(
                    mapper.Map<SortParametersDto>(sortParameters)
                )

            if isNull comments then
                return (this.NotFound() :> IActionResult)

            else
                return
                    (this.Ok(
                        mapper.Map<PagedCommentsResponse>(comments)
                    ) :> IActionResult)
        }

    [<ProducesResponseType(typeof<List<CommentResponse>>, 200)>]
    [<ProducesResponseType(404)>]
    [<HttpPost(ApiRoutes.CommentsController.GetCommentsByPost)>]
    [<Cached(600)>]
    member this.GetCommentsByPostAsync(
        [<FromRoute>] id : int,
        [<FromBody>] sortParameters : SortParametersRequest
    )
        : Task<IActionResult> =

        task {

            let sortParameters =
                if isNull sortParameters then
                    SortParametersRequest()
                else
                    sortParameters

            if not sortParameters.CurrentPage.HasValue then
                sortParameters.CurrentPage <- Nullable 1

            sortParameters.PageSize <- 10

            let! comments =
                commentsService.GetPagedCommentsByPostId(
                    id,
                    mapper.Map<SortParametersDto>(sortParameters)
                )

            if isNull comments then
                return (this.NotFound() :> IActionResult)

            else
                return
                    (this.Ok(
                        mapper.Map<List<CommentResponse>>(comments)
                    ) :> IActionResult)
        }

    [<ProducesResponseType(typeof<CommentResponse>, 200)>]
    [<ProducesResponseType(404)>]
    [<HttpGet("{id:int}", Name = ApiRoutes.CommentsController.GetComment)>]
    member this.GetComment(
        [<FromRoute>] id : int
    )
        : Task<IActionResult> =

        task {

            let! comment =
                commentsService.GetCommentAsync(id)

            if isNull comment then
                return (this.NotFound() :> IActionResult)

            else
                return
                    (this.Ok(
                        mapper.Map<CommentResponse>(comment)
                    ) :> IActionResult)
        }