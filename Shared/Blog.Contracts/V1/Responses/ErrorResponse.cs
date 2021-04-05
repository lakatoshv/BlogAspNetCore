namespace Blog.Contracts.V1.Responses
{
    using System.Collections.Generic;

    /// <summary>
    /// Error response.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
        /// </summary>
        public ErrorResponse() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        public ErrorResponse(ErrorModel error)
        {
            Errors.Add(error);
        }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    }
}
