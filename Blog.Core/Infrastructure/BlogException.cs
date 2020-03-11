namespace Blog.Core.Infrastructure
{
    using System;
    using System.Runtime.Serialization;
    [Serializable]
    public class BlogException : Exception
    {
        public BlogException()
        {
        }

        public BlogException(string message)
            : base(message)
        {
        }

        public BlogException(string messageFormat, params object[] args)
            : base(string.Format(messageFormat, args))
        {
        }

        protected BlogException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public BlogException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
