// <copyright file="SortParametersDto.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Dtos
{
    /// <summary>
    /// Sort parameters dto.
    /// </summary>
    public class SortParametersDto
    {
        /// <summary>
        /// Gets or sets orderBy.
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// Gets or sets sortBy.
        /// </summary>
        public string SortBy { get; set; }

        /// <summary>
        /// Gets or sets currentPage.
        /// </summary>
        public int? CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets pageSize.
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// Gets or sets displayType.
        /// </summary>
        public string DisplayType { get; set; }
    }
}
