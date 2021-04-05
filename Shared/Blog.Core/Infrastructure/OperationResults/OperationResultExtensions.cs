namespace Blog.Core.Infrastructure.OperationResults
{
    using System;
    using System.Linq;
    using System.Text;
    using Blog.Core.Infrastructure.OperationResults.Interfaces;

    /// <summary>
    /// Operation result extensions.
    /// </summary>
    public static class OperationResultExtensions
    {
        /// <summary>
        /// Adds the information.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <returns>IHaveDataObject.</returns>
        public static IHaveDataObject AddInfo(
          this OperationResult source,
          string message)
        {
            source.AppendLog(message);
            source.Metadata = new Metadata(source, message);

            return source.Metadata;
        }

        /// <summary>
        /// Adds the success.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <returns>IHaveDataObject.</returns>
        public static IHaveDataObject AddSuccess(
          this OperationResult source,
          string message)
        {
            source.AppendLog(message);
            source.Metadata = new Metadata(source, message, MetadataType.Success);

            return source.Metadata;
        }

        /// <summary>
        /// Adds the warning.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <returns>IHaveDataObject.</returns>
        public static IHaveDataObject AddWarning(
          this OperationResult source,
          string message)
        {
            source.AppendLog(message);
            source.Metadata = new Metadata(source, message, MetadataType.Warning);

            return source.Metadata;
        }

        /// <summary>
        /// Adds the error.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <returns>IHaveDataObject.</returns>
        public static IHaveDataObject AddError(
          this OperationResult source,
          string message)
        {
            source.AppendLog(message);
            source.Metadata = new Metadata(source, message, MetadataType.Error);

            return source.Metadata;
        }

        /// <summary>
        /// Adds the error.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>IHaveDataObject.</returns>
        public static IHaveDataObject AddError(
          this OperationResult source,
          Exception exception)
        {
            source.Exception = exception;
            source.Metadata = new Metadata(source, exception?.Message, MetadataType.Error);
            if (exception != null)
            {
                source.AppendLog(exception.Message);
            }

            return source.Metadata;
        }

        /// <summary>
        /// Adds the error.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>IHaveDataObject.</returns>
        public static IHaveDataObject AddError(
          this OperationResult source,
          string message,
          Exception exception)
        {
            source.Exception = exception;
            source.Metadata = new Metadata(source, message, MetadataType.Error);
            if (!string.IsNullOrEmpty(message))
            {
                source.AppendLog(message);
            }

            if (exception != null)
            {
                source.AppendLog(exception.Message);
            }

            return source.Metadata;
        }

        /// <summary>
        /// Gets the metadata messages.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>string.</returns>
        /// <exception cref="ArgumentNullException">Exception.</exception>
        public static string GetMetadataMessages(this OperationResult source)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }

            var sb = new StringBuilder();
            if (source.Metadata != null)
            {
                sb.AppendLine(source.Metadata.Message ?? string.Empty);
            }

            if (!source.Logs.Any())
            {
                return sb.ToString();
            }

            source.Logs.ToList().ForEach(x => sb.AppendLine("Log: " + x));

            return sb.ToString();
        }
    }
}