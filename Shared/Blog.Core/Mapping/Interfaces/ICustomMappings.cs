﻿// <copyright file="ICustomMappings.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Mapping.Interfaces;

using AutoMapper;

/// <summary>
/// Custom mappings interface.
/// </summary>
public interface ICustomMappings
{
    /// <summary>
    /// Create mappings.
    /// </summary>
    /// <param name="configuration">configuration.</param>
    void CreateMappings(IMapperConfigurationExpression configuration);
}