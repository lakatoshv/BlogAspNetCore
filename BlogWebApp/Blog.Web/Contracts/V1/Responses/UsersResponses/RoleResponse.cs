namespace Blog.Web.Contracts.V1.Responses.UsersResponses
{
    /// <summary>
    /// Role response.
    /// </summary>
    public class RoleResponse
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public string Role { get; set; }
    }
}