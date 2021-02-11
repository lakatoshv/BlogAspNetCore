// <copyright file="IMessagesService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Interfaces
{
    using Blog.Data.Models;
    using Blog.Services.GeneralService;

    /// <summary>
    /// Messages service interface.
    /// </summary>
    /// <seealso cref="IGeneralService{Message}" />
    public interface IMessagesService : IGeneralService<Message>
    {
    }
}