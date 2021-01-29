namespace Blog.Contracts.V1.Responses
{
    /// <summary>
    /// Created response.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CreatedResponse<T>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public T Id { get; set; }
    }
}