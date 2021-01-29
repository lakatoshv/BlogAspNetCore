// <copyright file="AutoMapperConfig.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Core.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using AutoMapper;
    using Interfaces;

    /// <summary>
    /// AutoMapper Configuration.
    /// </summary>
    public static class AutoMapperConfig
    {
        /// <summary>
        /// Register mappings.
        /// </summary>
        /// <param name="assemblies">assemblies.</param>
        public static void RegisterMappings(params Assembly[] assemblies)
        {
            var types = assemblies.SelectMany(a => a.GetExportedTypes()).ToList();

            Mapper.Initialize(configuration =>
            {
                RegisterStandardFromMappings(configuration, types);

                RegisterStandardToMappings(configuration, types);

                RegisterCustomMaps(configuration, types);
            });
        }

        /// <summary>
        /// Register standard from mappings.
        /// </summary>
        /// <param name="configuration">configuration.</param>
        /// <param name="types">types.</param>
        private static void RegisterStandardFromMappings(IProfileExpression configuration, IEnumerable<Type> types)
        {
            var maps = GetFromMaps(types);

            CreateMappings(configuration, maps);
        }

        /// <summary>
        /// Register standard to mappings.
        /// </summary>
        /// <param name="configuration">configuration.</param>
        /// <param name="types">types.</param>
        private static void RegisterStandardToMappings(IProfileExpression configuration, IEnumerable<Type> types)
        {
            var maps = GetToMaps(types);

            CreateMappings(configuration, maps);
        }

        /// <summary>
        /// Register custom maps.
        /// </summary>
        /// <param name="configuration">configuration.</param>
        /// <param name="types">types.</param>
        private static void RegisterCustomMaps(IMapperConfigurationExpression configuration, IEnumerable<Type> types)
        {
            var maps = GetCustomMappings(types);

            CreateMappings(configuration, maps);
        }

        /// <summary>
        /// Get custom mappings.
        /// </summary>
        /// <param name="types">types.</param>
        /// <returns>IEnumerable.</returns>
        private static IEnumerable<ICustomMappings> GetCustomMappings(IEnumerable<Type> types)
        {
            var customMaps = from t in types
                             from i in t.GetTypeInfo().GetInterfaces()
                             where typeof(ICustomMappings).GetTypeInfo().IsAssignableFrom(t) &&
                                !t.GetTypeInfo().IsAbstract &&
                                !t.GetTypeInfo().IsInterface
                             select (ICustomMappings)Activator.CreateInstance(t);

            return customMaps;
        }

        /// <summary>
        /// Get from maps.
        /// </summary>
        /// <param name="types">types.</param>
        /// <returns>IEnumerable.</returns>
        private static IEnumerable<TypesMap> GetFromMaps(IEnumerable<Type> types)
        {
            var fromMaps = from t in types
                           from i in t.GetTypeInfo().GetInterfaces()
                           where i.GetTypeInfo().IsGenericType &&
                                 i.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
                                 !t.GetTypeInfo().IsAbstract &&
                                 !t.GetTypeInfo().IsInterface
                           select new TypesMap
                           {
                               Source = i.GetTypeInfo().GetGenericArguments()[0],
                               Destination = t,
                           };

            return fromMaps;
        }

        /// <summary>
        /// Get to maps.
        /// </summary>
        /// <param name="types">types.</param>
        /// <returns>IEnumerable.</returns>
        private static IEnumerable<TypesMap> GetToMaps(IEnumerable<Type> types)
        {
            var toMaps = from t in types
                         from i in t.GetTypeInfo().GetInterfaces()
                         where i.GetTypeInfo().IsGenericType &&
                               i.GetTypeInfo().GetGenericTypeDefinition() == typeof(IMapTo<>) &&
                               !t.GetTypeInfo().IsAbstract &&
                               !t.GetTypeInfo().IsInterface
                         select new TypesMap
                         {
                             Source = t,
                             Destination = i.GetTypeInfo().GetGenericArguments()[0],
                         };

            return toMaps;
        }

        /// <summary>
        /// Create mappings.
        /// </summary>
        /// <param name="configuration">configuration.</param>
        /// <param name="maps">maps.</param>
        private static void CreateMappings(IProfileExpression configuration, IEnumerable<TypesMap> maps)
        {
            foreach (var map in maps)
            {
                configuration.CreateMap(map.Source, map.Destination);
            }
        }

        /// <summary>
        /// Create mappings.
        /// </summary>
        /// <param name="configuration">configuration.</param>
        /// <param name="maps">maps.</param>
        private static void CreateMappings(IMapperConfigurationExpression configuration, IEnumerable<ICustomMappings> maps)
        {
            foreach (var map in maps)
            {
                map.CreateMappings(configuration);
            }
        }
    }
}
