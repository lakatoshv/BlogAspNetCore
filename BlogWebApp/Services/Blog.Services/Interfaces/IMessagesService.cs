// <copyright file="IMessagesService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.EntityServices.Interfaces;

using Data.Models;
using GeneralService;

/// <summary>
/// Messages service interface.
/// </summary>
/// <seealso cref="IGeneralService{Message}" />
public interface IMessagesService : IGeneralService<Message>;