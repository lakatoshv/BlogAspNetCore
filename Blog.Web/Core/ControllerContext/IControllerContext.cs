namespace Blog.Web.Core.ControllerContext
{
    using Blog.Data.Models;

    /// <summary>
    /// Controller context interface.
    /// </summary>
    public interface IControllerContext
    {
        /// <summary>
        /// Gets or sets current user.
        /// </summary>
        ApplicationUser CurrentUser { get; set; }
    }
}
