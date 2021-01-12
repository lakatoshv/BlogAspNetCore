// <copyright file="SearchParametersDto.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Dtos
{
    /// <summary>
    /// Search parameters dto.
    /// </summary>
    public class SearchParametersDto
    {
        /// <summary>
        /// Gets or sets search.
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        /// Gets or sets sortParameters.
        /// </summary>
        public SortParametersDto SortParameters { get; set; }
    }
}
