// <copyright file="BlogException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Infrastructure
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Blog exception.
    /// </summary>
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
        /// <param name="message">message.</param>
        public BlogException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogException"/> class.
        /// </summary>
        /// <param name="messageFormat">messageFormat.</param>
        /// <param name="args">args.</param>
        public BlogException(string messageFormat, params object[] args)
            : base(string.Format(messageFormat, args))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogException"/> class.
        /// </summary>
        /// <param name="message">message.</param>
        /// <param name="innerException">innerException.</param>
        public BlogException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogException"/> class.
        /// </summary>
        /// <param name="info">info.</param>
        /// <param name="context">context.</param>
        protected BlogException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
