// <copyright file="MessagesService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services;

using Data.Models;
using Data.Repository;
using GeneralService;
using Interfaces;

/// <summary>
/// Messages service.
/// </summary>
/// <seealso cref="GeneralService{Message}" />
/// <seealso cref="IMessagesService" />
public class MessagesService : GeneralService<Message>, IMessagesService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessagesService"/> class.
    /// </summary>
    /// <param name="repo">The repo.</param>
    public MessagesService(
        IRepository<Message> repo)
        : base(repo)
    {
    }
}