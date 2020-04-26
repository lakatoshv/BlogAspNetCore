// <copyright file="ICommentsService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Interfaces
{
    using Data.Models;
    using GeneralService;

    /// <summary>
    /// Comments service interface.
    /// </summary>
    /// <seealso cref="IGeneralService{Comment}" />
    public interface ICommentsService : IGeneralService<Comment>
    {
    }
}