namespace AuthService.Core.Infrastructure
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Blog exception.
    /// </summary>
    /// <seealso cref="Exception" />
    [Serializable]
    public class BlogException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogException"/> class.
        /// </summary>
        public BlogException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public BlogException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogException"/> class.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="args">The arguments.</param>
        public BlogException(string messageFormat, params object[] args)
            : base(string.Format(messageFormat, args))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogException"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        protected BlogException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public BlogException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
