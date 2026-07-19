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

    [<HttpPost(ApiRoutes.CommentsController.CreateComment)>]
    [<ProducesResponseType(typeof<CommentResponse>, 201)>]
    [<ProducesResponseType(typeof<ModelStateDictionary>, 400)>]
    [<Authorize>]
    member this.CreateAsync(
        [<FromBody>] request : CreateCommentRequest
    )
        : Task<IActionResult> =

        task {

            if not this.ModelState.IsValid then
                return this.BadModel(this.ModelState)

            else

                let comment =
                    mapper.Map<Comment>(request)

                comment.CreatedAt <- DateTime.Now

                do!
                    commentsService.InsertAsync(comment)

                let response = CreatedResponse<int>()
                response.Id <- comment.Id

                let baseUrl =
                    $"{this.HttpContext.Request.Scheme}://{this.HttpContext.Request.Host.ToUriComponent()}"

                let locationUrl =
                    $"{baseUrl}/{ApiRoutes.CommentsController.Comments}/{ApiRoutes.CommentsController.GetComment}/{comment.Id}"

                return
                    (this.CreatedAtRoute(locationUrl, response)
                        :> IActionResult)
        }

    [<HttpPut(ApiRoutes.CommentsController.EditComment)>]
    [<ProducesResponseType(typeof<CommentResponse>, 204)>]
    [<ProducesResponseType(400)>]
    [<ProducesResponseType(404)>]
    [<Authorize>]
    member this.EditAsync(
        [<FromRoute>] id : int,
        [<FromBody>] request : UpdateCommentRequest
    )
        : Task<IActionResult> =

        task {

            let! originComment =
                commentsService.GetCommentAsync(id)

            if isNull originComment then
                return (this.NotFound() :> IActionResult)

            else

                if not (originComment.UserId.Equals(request.UserId)) then
                    return (this.NotFound() :> IActionResult)

                else

                    request.CreatedAt <- DateTime.Now

                    let updatedComment =
                        mapper.Map(request, originComment)

                    do!
                        commentsService.UpdateAsync(updatedComment)

                    let! comment =
                        commentsService.GetCommentAsync(id)

                    return
                        (this.Ok(
                            mapper.Map<CommentResponse>(comment)
                        ) :> IActionResult)
        }

    [<HttpDelete(ApiRoutes.CommentsController.DeleteComment)>]
    [<ProducesResponseType(typeof<CreatedResponse<int>>, 200)>]
    [<ProducesResponseType(404)>]
    member this.DeleteAsync(
        [<FromRoute>] id : int
    )
        : Task<IActionResult> =

        task {

            let! comment =
                commentsService.GetCommentAsync(id)

            if isNull comment then
                return (this.NotFound() :> IActionResult)

            else

                do!
                    commentsService.DeleteAsync(comment)

                let response = CreatedResponse<int>()
                response.Id <- id

                return
                    (this.Ok(response)
                        :> IActionResult)
        }