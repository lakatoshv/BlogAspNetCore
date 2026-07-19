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

    [<HttpGet>]
    [<ProducesResponseType(typeof<List<MessageResponse>>, 200)>]
    [<ProducesResponseType(404)>]
    [<Cached(600)>]
    member this.Index() : Task<IActionResult> =
        task {

            let! messages =
                messagesService.GetAllAsync()

            if isNull messages then
                return (this.NotFound() :> IActionResult)

            else
                return
                    (this.Ok(
                        mapper.Map<List<MessageResponse>>(messages.ToList())
                    ) :> IActionResult)
        }

    [<HttpGet(ApiRoutes.MessagesController.GetRecipientMessages)>]
    [<ProducesResponseType(typeof<List<MessageResponse>>, 200)>]
    [<ProducesResponseType(404)>]
    [<Cached(600)>]
    member this.GetRecipientMessages([<FromRoute>] recipientId : string)
        : Task<IActionResult> =
        task {

            let specification =
                MessageSpecification(fun x ->
                    x.RecipientId.ToLower().Equals(recipientId.ToLower()))

            let! messages =
                messagesService.GetAllAsync(specification)

            if isNull messages then
                return (this.NotFound() :> IActionResult)

            else
                return
                    (this.Ok(
                        mapper.Map<List<MessageResponse>>(messages.ToList())
                    ) :> IActionResult)
        }

    [<HttpGet(ApiRoutes.MessagesController.GetSenderMessages)>]
    [<ProducesResponseType(typeof<List<MessageResponse>>, 200)>]
    [<ProducesResponseType(404)>]
    [<Cached(600)>]
    member this.GetSenderMessages([<FromRoute>] senderEmail : string)
        : Task<IActionResult> =
        task {

            let specification =
                MessageSpecification(fun x ->
                    x.SenderEmail.ToLower().Equals(senderEmail.ToLower()))

            let! messages =
                messagesService.GetAllAsync(specification)

            if isNull messages then
                return (this.NotFound() :> IActionResult)

            else
                return
                    (this.Ok(
                        mapper.Map<List<MessageResponse>>(messages.ToList())
                    ) :> IActionResult)
        }

    [<ProducesResponseType(typeof<MessageResponse>, 200)>]
    [<ProducesResponseType(404)>]
    [<HttpGet(ApiRoutes.MessagesController.Show)>]
    member this.Show([<FromRoute>] id : int)
        : Task<IActionResult> =
        task {

            let! message =
                messagesService.FindAsync(id)

            if isNull message then
                return (this.NotFound() :> IActionResult)

            else
                return
                    (this.Ok(
                        mapper.Map<MessageResponse>(message)
                    ) :> IActionResult)
        }

    [<HttpPost>]
    [<ProducesResponseType(201)>]
    [<ProducesResponseType(400)>]
    member this.CreateAsync([<FromBody>] request : CreateMessageRequest)
        : Task<IActionResult> =
        task {

            if not (isNull this.CurrentUser) then
                request.SenderId <- this.CurrentUser.Id

            do!
                messagesService.InsertAsync(
                    mapper.Map<Message>(request)
                )

            return (this.Ok() :> IActionResult)
        }

    [<HttpPut("{id:int}")>]
    [<Authorize>]
    [<ProducesResponseType(typeof<MessageResponse>, 204)>]
    [<ProducesResponseType(typeof<obj>, 400)>]
    [<ProducesResponseType(404)>]
    member this.EditAsync(
        [<FromRoute>] id : int,
        [<FromBody>] request : UpdateMessageRequest
    )
        : Task<IActionResult> =
        task {

            if isNull this.CurrentUser then
                return this.BadError("Unauthorized")

            elif
                not (request.SenderId.Equals(this.CurrentUser.Id))
                || not (request.RecipientId.Equals(this.CurrentUser.Id))
            then
                return this.BadError("You are not an author or recipient of the message.")

            else

                do!
                    messagesService.UpdateAsync(
                        mapper.Map<Message>(request)
                    )

                let! message =
                    messagesService.FindAsync(id)

                return
                    (this.Ok(
                        mapper.Map<MessageResponse>(message)
                    ) :> IActionResult)
        }

    [<HttpDelete("{id:int}")>]
    [<Authorize>]
    [<ProducesResponseType(typeof<CreatedResponse<int>>, 200)>]
    [<ProducesResponseType(404)>]
    member this.DeleteAsync(
        [<FromRoute>] id : int,
        [<FromBody>] authorId : string
    ) : Task<IActionResult> =

        task {

            if isNull this.CurrentUser then
                return this.BadError("Unauthorized")

            else

                let! message =
                    messagesService.FindAsync(id)

                if message.SenderId <> this.CurrentUser.Id
                    && message.RecipientId <> this.CurrentUser.Id then

                    return
                        this.BadRequest(
                            {| ErrorMessage =
                                "You are not an author of the post." |}
                        )
                        :> IActionResult

                else

                    do!
                        messagesService.DeleteAsync(message)

                    let response =
                        CreatedResponse<int>(Id = id)

                    return this.Ok(response) :> IActionResult
        }