using Blog.Services.ControllerContext;

namespace Blog.Web.Controllers
{
    using Data.Models;
    using Blog.Services.Core.Utilities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    /// <summary>
    /// Base controller.
    /// </summary>
    /// <seealso cref="Controller" />
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// The controller context.
        /// </summary>
        protected new readonly IControllerContext ControllerContext;

        /// <summary>
        /// The current user.
        /// </summary>
        protected ApplicationUser CurrentUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="workContext">The work context.</param>
        protected BaseController(IControllerContext workContext)
        {
            ControllerContext = workContext;
            CurrentUser = ControllerContext.CurrentUser;
        }

        /// <summary>
        /// Bad model state.
        /// </summary>
        /// <param name="modelState">State of the model.</param>
        /// <returns></returns>
        protected BadRequestObjectResult Bad(ModelStateDictionary modelState)
        {
            return BadRequest(modelState);
        }

        /// <summary>
        /// Bad model state.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        protected BadRequestObjectResult Bad(string name, string error)
        {
            ModelState.AddError(name, error);
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Bad result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        protected BadRequestObjectResult Bad(IdentityResult result)
        {
            ModelState.AddErrors(result);
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Bad error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        protected BadRequestObjectResult Bad(string error)
        {
            return Bad("BadRequest", error);
        }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        protected string UserName => User.GetUserName() ?? string.Empty;
    }
}
