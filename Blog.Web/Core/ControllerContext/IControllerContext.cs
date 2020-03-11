namespace Blog.Web.Core.ControllerContext
{
    using Blog.Data.Models;

    public interface IControllerContext
    {
        ApplicationUser CurrentUser { get; set; }
    }
}
