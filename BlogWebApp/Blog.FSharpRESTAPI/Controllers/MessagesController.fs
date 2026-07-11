namespace Blog.FSharpRESTAPI.Controllers

open Blog.EntityServices.ControllerContext
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open AutoMapper
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open Blog.Data.Models
open Blog.Contracts.V1.Requests.MessagesRequests
open Blog.Contracts.V1.Responses
open Blog.Core.Consts
open Blog.Data.Specifications
open Blog.EntityServices.Interfaces
open Blog.Contracts.V1
open Blog.FSharpRESTAPI.Cache

[<Route(ApiRoutes.MessagesController.Messages)>]
[<ApiController>]
[<AllowAnonymous>]
[<Produces(Consts.JsonType)>]
type MessagesController(
    controllerContext : IControllerContext,
    messagesService : IMessagesService,
    mapper : IMapper
) =
    inherit BaseController(controllerContext)