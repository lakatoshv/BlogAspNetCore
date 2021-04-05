// <copyright file="IHaveDataObject.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Infrastructure.OperationResults.Interfaces
{
    /// <summary>
    /// Have data object interface.
    /// </summary>
    public interface IHaveDataObject
    {
        /// <summary>
        /// Adds the data.
        /// </summary>
        /// <param name="data">The data.</param>
        void AddData(object data);
    }
}