// <copyright file="CachingDefaults.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Caching
{
    /// <summary>
    /// Caching defaults.
    /// </summary>
    public static class CachingDefaults
    {
        /// <summary>
        /// Gets cache time.
        /// </summary>
        public static int CacheTime => 60;

        /// <summary>
        /// Gets redis data protection key.
        /// </summary>
        public static string RedisDataProtectionKey => "Blog.DataProtectionKeys";
    }
}
