// <copyright file="OperationResult{T}.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Infrastructure.OperationResults
{
    using System;

    /// <summary>
    /// OperationResult{T}.
    /// </summary>
    /// <typeparam name="T">T.</typeparam>
    [Serializable]
    public class OperationResult<T> : OperationResult
    {
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public T Result { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="OperationResult{T}"/> is ok.
        /// </summary>
        /// <value>
        ///   <c>true</c> if ok; otherwise, <c>false</c>.
        /// </value>
        public bool Ok
        {
            get
            {
                if (this.Metadata == null)
                {
                    return this.Exception == null && this.Result != null;
                }

                if (this.Exception != null || this.Result == null)
                {
                    return false;
                }

                var metadata = this.Metadata;

                return metadata == null || metadata.Type != MetadataType.Error;
            }
        }
    }
}