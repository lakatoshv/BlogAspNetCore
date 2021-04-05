namespace Blog.Data.UnitOfWork
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Save changes result.
    /// </summary>
    public class SaveChangesResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaveChangesResult"/> class.
        /// </summary>
        public SaveChangesResult() => this.Messages = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveChangesResult"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public SaveChangesResult(string message)
            : this()
            => this.AddMessage(message);

        /// <summary>
        /// Gets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public List<string> Messages { get; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is ok.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is ok; otherwise, <c>false</c>.
        /// </value>
        public bool IsOk => this.Exception == null;

        /// <summary>
        /// Adds the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void AddMessage(string message) => this.Messages.Add(message);
    }
}