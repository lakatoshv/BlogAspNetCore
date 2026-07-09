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