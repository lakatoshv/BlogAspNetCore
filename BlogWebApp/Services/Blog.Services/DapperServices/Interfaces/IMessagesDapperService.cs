// <copyright file="IMessagesDapperService.cs" company="Blog">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Blog.Data.Models;
using Blog.EntityServices.GeneralService;

namespace Blog.EntityServices.DapperServices.Interfaces;

/// <summary>
/// Messages dapper service interface.
/// </summary>
/// <seealso cref="IGeneralDapperService{Message}" />
public interface IMessagesDapperService: IGeneralDapperService<Message>;