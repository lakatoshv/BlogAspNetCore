namespace Blog.Web.Controllers
{
    using Data.Models;
    using Blog.Services.Core.Utilities;
    using Core.ControllerContext;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public abstract class BaseController : Controller
    {
        protected new readonly IControllerContext ControllerContext;
        
        protected ApplicationUser CurrentUser;

        protected BaseController(IControllerContext workContext)
        {
            ControllerContext = workContext;
            CurrentUser = ControllerContext.CurrentUser;
        }

        protected BadRequestObjectResult Bad(ModelStateDictionary modelState)
        {
            return BadRequest(modelState);
        }

        protected BadRequestObjectResult Bad(string name, string error)
        {
            ModelState.AddError(name, error);
            return BadRequest(ModelState);
        }

        protected BadRequestObjectResult Bad(IdentityResult result)
        {
            ModelState.AddErrors(result);
            return BadRequest(ModelState);
        }

        protected BadRequestObjectResult Bad(string error)
        {
            return Bad("BadRequest", error);
        }

        protected string UserName
        {
            get
            {
                return User.GetUserName() ?? string.Empty;
            }
        }
    }
}
