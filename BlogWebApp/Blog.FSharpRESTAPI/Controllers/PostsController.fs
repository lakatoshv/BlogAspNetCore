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

    // --------------------------------------------------------------------
    // POST: Posts/get-posts
    // --------------------------------------------------------------------

    [<HttpPost(ApiRoutes.PostsController.GetPosts)>]
    [<ProducesResponseType(typeof<PagedPostsResponse>, 200)>]
    [<ProducesResponseType(404)>]
    [<Cached(600)>]
    member this.GetPosts
        (
            [<FromBody>] searchParameters : PostsSearchParametersRequest
        ) : Task<IActionResult> =

        task {

            searchParameters.SortParameters
                <- searchParameters.SortParameters
                |> Option.ofObj
                |> Option.defaultValue (SortParametersRequest())

            searchParameters.SortParameters.OrderBy
                <- searchParameters.SortParameters.OrderBy
                |> Option.ofObj
                |> Option.defaultValue "asc"

            searchParameters.SortParameters.SortBy
                <- searchParameters.SortParameters.SortBy
                |> Option.ofObj
                |> Option.defaultValue "Title"

            searchParameters.SortParameters.CurrentPage
                <- searchParameters.SortParameters.CurrentPage
                |> Option.ofNullable
                |> Option.defaultValue 1
                |> Nullable

            searchParameters.SortParameters.PageSize
                <- searchParameters.SortParameters.PageSize
                |> Option.ofNullable
                |> Option.defaultValue 10
                |> Nullable

            let! posts =
                mapper.Map<PostsSearchParametersDto>(searchParameters)
                |> postsService.GetPostsAsync

            if isNull posts then
                return this.NotFound() :> IActionResult
            else

                let response =
                    mapper.Map<PagedPostsResponse>(posts)

                return this.Ok(response) :> IActionResult
        }

    // --------------------------------------------------------------------
    // POST: Posts/user_posts/{id}
    // --------------------------------------------------------------------

    [<HttpPost(ApiRoutes.PostsController.UserPosts)>]
    [<ProducesResponseType(typeof<PagedPostsResponse>, 200)>]
    [<ProducesResponseType(404)>]
    [<Cached(600)>]
    member this.GetUserPosts
        (
            [<FromRoute>] id : string,
            [<FromBody>] searchParameters : PostsSearchParametersRequest
        ) : Task<IActionResult> =

        task {

            searchParameters.SortParameters
                <- searchParameters.SortParameters
                |> Option.ofObj
                |> Option.defaultValue (SortParametersRequest())

            searchParameters.SortParameters.OrderBy
                <- searchParameters.SortParameters.OrderBy
                |> Option.ofObj
                |> Option.defaultValue "asc"

            searchParameters.SortParameters.SortBy
                <- searchParameters.SortParameters.SortBy
                |> Option.ofObj
                |> Option.defaultValue "Title"

            searchParameters.SortParameters.CurrentPage
                <- searchParameters.SortParameters.CurrentPage
                |> Option.ofNullable
                |> Option.defaultValue 1
                |> Nullable

            searchParameters.SortParameters.PageSize <- Nullable 10

            let! posts =
                postsService.GetUserPostsAsync(
                    id,
                    mapper.Map<PostsSearchParametersDto>(searchParameters)
                )

            if isNull posts then
                return this.NotFound() :> IActionResult
            else

                let response =
                    mapper.Map<PagedPostsResponse>(posts)

                return this.Ok(response) :> IActionResult
        }

    // --------------------------------------------------------------------
    // GET: Posts/Show/{id}
    // --------------------------------------------------------------------

    [<ProducesResponseType(typeof<PostWithPagedCommentsResponse>, 200)>]
    [<ProducesResponseType(404)>]
    [<HttpGet(ApiRoutes.PostsController.Show)>]
    [<Cached(600)>]
    member this.Show([<FromRoute>] id : int) : Task<IActionResult> =
        task {

            let sortParameters = SortParametersDto()

            sortParameters.CurrentPage <- Nullable 1
            sortParameters.PageSize <- Nullable 10

            let! post =
                postsService.GetPost(id, sortParameters)

            if isNull post then
                return this.NotFound() :> IActionResult
            else

                let response =
                    mapper.Map<PostWithPagedCommentsResponse>(post)

                return this.Ok(response) :> IActionResult
        }

    // --------------------------------------------------------------------
    // POST: Posts
    // --------------------------------------------------------------------

    [<HttpPost>]
    [<Authorize>]
    [<ProducesResponseType(typeof<PostResponse>, 201)>]
    [<ProducesResponseType(typeof<obj>, 400)>]
    member this.CreateAsync(
        [<FromBody>] model : CreatePostRequest
    ) : Task<IActionResult> =

        task {

            if isNull this.CurrentUser then
                return this.BadError("Unauthorized")

            else

                if not this.ModelState.IsValid
                    || String.IsNullOrWhiteSpace(model.AuthorId) then

                    return (this.NotFound() :> IActionResult)

                else

                    model.AuthorId <- this.CurrentUser.Id

                    let postToCreate =
                        mapper.Map<Post>(model)

                    let tags =
                        mapper.Map<List<Tag>>(model.Tags.Distinct())

                    do!
                        postsService.InsertAsync(postToCreate, tags)

                    let response = CreatedResponse<int>()
                    response.Id <- postToCreate.Id

                    let baseUrl =
                        $"{this.HttpContext.Request.Scheme}://{this.HttpContext.Request.Host.ToUriComponent()}"

                    let locationUrl =
                        baseUrl + "/" +
                        ApiRoutes.PostsController.Show.Replace(
                            "{id}",
                            postToCreate.Id.ToString()
                        )

                    return
                        this.Created(locationUrl, response)
                        :> IActionResult
        }

    // --------------------------------------------------------------------
    // PUT: Posts/like-post
    // --------------------------------------------------------------------

    [<HttpPut(ApiRoutes.PostsController.LikePost)>]
    [<Authorize>]
    [<ProducesResponseType(typeof<PostViewResponse>, 204)>]
    [<ProducesResponseType(typeof<obj>, 400)>]
    [<ProducesResponseType(404)>]
    member this.LikePostAsync([<FromRoute>] id : int) : Task<IActionResult> =
        task {

            if isNull this.CurrentUser then
                return this.BadError("Unauthorized")

            else

                let! model =
                    postsService.GetPostAsync(id)

                if isNull model then
                    return (this.NotFound() :> IActionResult)

                else

                    model.Likes <- model.Likes + 1

                    do!
                        postsService.UpdateAsync(model)

                    let! post =
                        postsService.GetPostAsync(id)

                    let mappedPost =
                        mapper.Map<PostViewResponse>(post)

                    mappedPost.Author <-
                        mapper.Map<ApplicationUserResponse>(post.Author)

                    return
                        this.Ok(mappedPost)
                        :> IActionResult
        }

    // --------------------------------------------------------------------
    // PUT: Posts/dislike-post
    // --------------------------------------------------------------------

    [<HttpPut(ApiRoutes.PostsController.DislikePost)>]
    [<Authorize>]
    [<ProducesResponseType(typeof<PostViewResponse>, 204)>]
    [<ProducesResponseType(typeof<obj>, 400)>]
    [<ProducesResponseType(404)>]
    member this.DislikePostAsync([<FromRoute>] id : int) : Task<IActionResult> =
        task {

            if isNull this.CurrentUser then
                return this.BadError("Unauthorized")

            else

                let! model =
                    postsService.GetPostAsync(id)

                if isNull model then
                    return (this.NotFound() :> IActionResult)

                else

                    model.Dislikes <- model.Dislikes + 1

                    do!
                        postsService.UpdateAsync(model)

                    let! post =
                        postsService.GetPostAsync(id)

                    let mappedPost =
                        mapper.Map<PostViewResponse>(post)

                    mappedPost.Author <-
                        mapper.Map<ApplicationUserResponse>(post.Author)

                    return
                        this.Ok(mappedPost)
                        :> IActionResult
        }

    // --------------------------------------------------------------------
    // PUT: Posts/{id}
    // --------------------------------------------------------------------

    [<HttpPut("{id:int}")>]
    [<Authorize>]
    [<ProducesResponseType(typeof<PostViewResponse>, 204)>]
    [<ProducesResponseType(typeof<obj>, 400)>]
    [<ProducesResponseType(404)>]
    member this.EditAsync
        (
            [<FromRoute>] id : int,
            [<FromBody>] model : UpdatePostRequest
        ) : Task<IActionResult> =

        task {

            if isNull this.CurrentUser then
                return this.BadError("Unauthorized")

            else

                if not (model.AuthorId.Equals(this.CurrentUser.Id)) then
                    return this.BadError("You are not an author of the post.")

                else

                    let! post =
                        postsService.GetPostAsync(id)

                    if isNull post then
                        return (this.NotFound() :> IActionResult)

                    else

                        let updatedModel =
                            mapper.Map<UpdatePostRequest, Post>(
                                model,
                                post
                            )

                        updatedModel.Author <- this.CurrentUser

                        do!
                            postsService.UpdateAsync(updatedModel)

                        let tags =
                            mapper.Map<List<Tag>>(model.Tags)

                        do!
                            postsTagsRelationsService.AddTagsToExistingPost(
                                post.Id,
                                List<PostsTagsRelations>(post.PostsTagsRelations),
                                tags
                            )

                        let! postModel =
                            postsService.GetPostAsync(id)

                        let mappedPost =
                            mapper.Map<PostViewResponse>(postModel)

                        return
                            this.Ok(mappedPost)
                            :> IActionResult
        }