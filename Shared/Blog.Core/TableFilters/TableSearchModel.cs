// <copyright file="TableSearchModel.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Core.TableFilters
{
    /// <summary>
    /// Table search model.
    /// </summary>
    public class TableSearchModel
    {
        /// <summary>
        /// Gets or sets value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether regex.
        /// </summary>
        public bool Regex { get; set; }
    }
}
