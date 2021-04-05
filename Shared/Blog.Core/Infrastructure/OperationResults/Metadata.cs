// <copyright file="Metadata.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Infrastructure.OperationResults
{
    using System;
    using Blog.Core.Infrastructure.OperationResults.Interfaces;

    /// <summary>
    /// Metadata.
    /// </summary>
    /// <seealso cref="IMetadataMessage" />
    /// <seealso cref="IHaveDataObject" />
    [Serializable]
    public class Metadata : IMetadataMessage, IHaveDataObject
    {
        /// <summary>
        /// The source.
        /// </summary>
        private readonly OperationResult _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="Metadata"/> class.
        /// </summary>
        public Metadata() => this.Type = MetadataType.Info;

        /// <summary>
        /// Initializes a new instance of the <see cref="Metadata"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        public Metadata(OperationResult source, string message)
            : this()
        {
            this._source = source;
            this.Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Metadata"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        public Metadata(OperationResult source, string message, MetadataType type = MetadataType.Info)
        {
            this.Type = type;
            this._source = source;
            this.Message = message;
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public MetadataType Type { get; }

        /// <summary>
        /// Gets the data object.
        /// </summary>
        /// <value>
        /// The data object.
        /// </value>
        public object DataObject { get; private set; }

        /// <summary>
        /// Adds the data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void AddData(object data)
        {
            if (data is Exception exception && this._source.Metadata == null)
            {
                this._source.Metadata = new Metadata(this._source, exception.Message);
            }
            else
            {
                this._source.Metadata.DataObject = data;
            }
        }
    }
}