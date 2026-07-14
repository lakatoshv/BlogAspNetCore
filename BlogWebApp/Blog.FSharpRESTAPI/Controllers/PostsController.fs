namespace Blog.FSharpRESTAPI.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks

open AutoMapper

open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc

open Blog.Core.Consts
open Blog.Contracts.V1
open Blog.Contracts.V1.Requests
open Blog.Contracts.V1.Responses
open Blog.Contracts.V1.Responses.Chart

open Blog.Data.Models
open Blog.Services.Core.Dtos

open Blog.EntityServices.ControllerContext
open Blog.EntityServices.Interfaces

open Blog.FSharpRESTAPI.Cache
open Blog.Contracts.V1.Responses.PostsResponses
open Blog.Contracts.V1.Requests.PostsRequests
open Blog.Services.Core.Dtos.Posts
open Blog.Contracts.V1.Responses.UsersResponses

[<Route(ApiRoutes.PostsController.Posts)>]
[<ApiController>]
[<AllowAnonymous>]
[<Produces(Consts.JsonType)>]
type PostsController(
    controllerContext : IControllerContext,
    postsService : IPostsService,
    postsTagsRelationsService : IPostsTagsRelationsService,
    mapper : IMapper
) =
    inherit BaseController(controllerContext)

    // --------------------------------------------------------------------
    // GET: Posts
    // --------------------------------------------------------------------

    [<HttpGet>]
    [<ProducesResponseType(typeof<List<PostResponse>>, 200)>]
    [<ProducesResponseType(404)>]
    [<Cached(600)>]
    member this.Index() : Task<IActionResult> =
        task {

            let! posts =
                postsService.GetAllAsync()

            if isNull posts then
                return this.NotFound() :> IActionResult
            else

                let response =
                    mapper.Map<List<PostResponse>>(posts.ToList())

                return this.Ok(response) :> IActionResult
        }

    // --------------------------------------------------------------------
    // GET: Posts/posts-activity
    // --------------------------------------------------------------------

    [<HttpGet(ApiRoutes.PostsController.PostsActivity)>]
    [<ProducesResponseType(typeof<ChartDataModel>, 200)>]
    [<ProducesResponseType(404)>]
    [<Cached(600)>]
    member this.PostsActivity() : Task<IActionResult> =
        task {

            let! activity =
                postsService.GetPostsActivity()

            if isNull activity then
                return this.NotFound() :> IActionResult
            else
                return this.Ok(activity) :> IActionResult
        }