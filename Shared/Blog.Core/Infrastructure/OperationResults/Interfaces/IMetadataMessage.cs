// <copyright file="IMetadataMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Infrastructure.OperationResults.Interfaces
{
    /// <summary>
    /// Metadata message interface.
    /// </summary>
    /// <seealso cref="IHaveDataObject" />
    public interface IMetadataMessage : IHaveDataObject
    {
        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        string Message { get; }

        /// <summary>
        /// Gets the data object.
        /// </summary>
        /// <value>
        /// The data object.
        /// </value>
        object DataObject { get; }
    }
}