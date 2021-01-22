namespace Blog.Web.Contracts.V1.Requests.MessagesRequests
{
    /// <summary>
    /// Update message request.
    /// </summary>
    public class UpdateMessageRequest
    {
        /// <summary>
        /// Gets or sets the sender identifier.
        /// </summary>
        /// <value>
        /// The sender identifier.
        /// </value>
        public string SenderId { get; set; }

        /// <summary>
        /// Gets or sets the recipient identifier.
        /// </summary>
        /// <value>
        /// The recipient identifier.
        /// </value>
        public string RecipientId { get; set; }

        /// <summary>
        /// Gets or sets the sender email.
        /// </summary>
        /// <value>
        /// The sender email.
        /// </value>
        public string SenderEmail { get; set; }

        /// <summary>
        /// Gets or sets the name of the sender.
        /// </summary>
        /// <value>
        /// The name of the sender.
        /// </value>
        public string SenderName { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        public int MessageType { get; set; }
    }
}