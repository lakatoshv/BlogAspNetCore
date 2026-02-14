// <copyright file="MessagesService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.EntityServices.EntityFrameworkServices;

using Interfaces;
using Data.Models;
using Data.Repository;
using GeneralService;

/// <summary>
/// Messages service.
/// </summary>
/// <seealso cref="GeneralService{Message}" />
/// <seealso cref="IMessagesService" />
/// <remarks>
/// Initializes a new instance of the <see cref="MessagesService"/> class.
/// </remarks>
/// <param name="repo">The repo.</param>
public class MessagesService(IRepository<Message> repo)
    : GeneralService<Message>(repo), IMessagesService;