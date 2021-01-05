// <copyright file="IMessagesService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Blog.Data.Models;
using Blog.Services.GeneralService;

namespace Blog.Services.Interfaces
{
    /// <summary>
    /// Messages service interface.
    /// </summary>
    /// <seealso cref="IGeneralService{Message}" />
    public interface IMessagesService : IGeneralService<Message>
    {
    }
}