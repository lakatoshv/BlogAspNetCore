// <copyright file="MessagesDapperService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Blog.Data.Models;
using Blog.Data.Repository;
using Blog.EntityServices.DapperServices.Interfaces;
using Blog.EntityServices.GeneralService;
using Blog.EntityServices.Interfaces;

namespace Blog.EntityServices.DapperServices;

/// <summary>
/// Messages dapper service.
/// </summary>
/// <seealso cref="GeneralService{Message}" />
/// <seealso cref="IMessagesService" />
/// <remarks>
/// Initializes a new instance of the <see cref="MessagesDapperService"/> class.
/// </remarks>
/// <param name="repo">The repo.</param>
public class MessagesDapperService(IDapperRepository<Message> repo)
    : GeneralDapperService<Message>(repo), IMessagesDapperService;