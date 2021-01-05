namespace Blog.Services.Security
{
    /// <summary>
    /// Permission service interface.
    /// </summary>
    public interface IPermissionService
    {
        /// <summary>
        /// Authorize permission.
        /// </summary>
        /// <returns>bool.</returns>
        bool Authorize();
    }
}
