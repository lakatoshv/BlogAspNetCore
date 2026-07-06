namespace Blog.FSharpRESTAPI.Controllers

open System

open Microsoft.AspNetCore.Identity
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc.ModelBinding

open Blog.EntityServices.ControllerContext

[<AbstractClass>]
type BaseController(controllerContext: IControllerContext) =
    inherit Controller()

    [<NonAction>]
    member _.CurrentUser =
        controllerContext.CurrentUser

    [<NonAction>]
    member this.UserName =
        if isNull this.User || isNull this.User.Identity then
            String.Empty
        else
            this.User.Identity.Name

    [<NonAction>]
    member this.BadModel(modelState: ModelStateDictionary) : IActionResult =
        this.BadRequest(modelState) :> IActionResult

    [<NonAction>]
    member this.BadError(error: string) : IActionResult =
        this.ModelState.AddModelError("BadRequest", error)
        this.BadRequest(this.ModelState) :> IActionResult

    [<NonAction>]
    member this.BadIdentity(result: IdentityResult) : IActionResult =

        result.Errors
        |> Seq.iter (fun e ->
            this.ModelState.AddModelError(e.Code, e.Description)
        )

        this.BadRequest(this.ModelState) :> IActionResult