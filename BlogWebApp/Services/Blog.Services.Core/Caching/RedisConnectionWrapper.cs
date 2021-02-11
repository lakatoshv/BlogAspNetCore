// <copyright file="RedisConnectionWrapper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Caching
{
    using System;
    using System.Linq;
    using System.Net;
    using Blog.Core.Configuration;
    using Blog.Services.Core.Caching.Interfaces;
    using RedLockNet.SERedis;
    using RedLockNet.SERedis.Configuration;
    using StackExchange.Redis;

    /// <summary>
    /// Redis connection wrapper.
    /// </summary>
    public class RedisConnectionWrapper : IRedisConnectionWrapper, ILocker
    {
        /// <summary>
        /// Blog configuration.
        /// </summary>
        private readonly BlogConfiguration config;

        /// <summary>
        /// Lazy connection.
        /// </summary>
        private readonly Lazy<string> connectionString;

        /// <summary>
        /// Lock object.
        /// </summary>
        private readonly object @lock = new object();

        /// <summary>
        /// RedLock factory.
        /// </summary>
        private readonly RedLockFactory redisLockFactory;

        /// <summary>
        /// Connection multiplexer.
        /// </summary>
        private volatile ConnectionMultiplexer connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisConnectionWrapper"/> class.
        /// </summary>
        /// <param name="config">config.</param>
        public RedisConnectionWrapper(BlogConfiguration config)
        {
            this.config = config;
            this.connectionString = new Lazy<string>(this.GetConnectionString);
            this.redisLockFactory = this.CreateRedisLockFactory();
        }

        /// <summary>
        /// Get database.
        /// </summary>
        /// <param name="db">db.</param>
        /// <returns>IDatabase.</returns>
        public IDatabase GetDatabase(int? db = null)
        {
            return this.GetConnection().GetDatabase(db ?? -1);
        }

        /// <inheritdoc cref="IRedisConnectionWrapper"/>
        EndPoint[] IRedisConnectionWrapper.GetEndPoints()
        {
            return this.GetEndPoints();
        }

        /// <inheritdoc cref="IRedisConnectionWrapper"/>
        public IServer GetServer(EndPoint endPoint)
        {
            return this.GetConnection().GetServer(endPoint);
        }

        /// <inheritdoc cref="IRedisConnectionWrapper"/>
        IDatabase IRedisConnectionWrapper.GetDatabase(int? db)
        {
            return this.GetDatabase(db);
        }

        /// <summary>
        /// Get EndPoints.
        /// </summary>
        /// <returns>EndPoint[].</returns>
        public EndPoint[] GetEndPoints()
        {
            return this.GetConnection().GetEndPoints();
        }

        /// <summary>
        /// Flush database.
        /// </summary>
        /// <param name="db">db.</param>
        public void FlushDatabase(int? db = null)
        {
            var endPoints = this.GetEndPoints();

            foreach (var endPoint in endPoints)
            {
                this.GetServer(endPoint).FlushDatabase(db ?? -1);
            }
        }

        /// <inheritdoc cref="ILocker"/>
        public bool PerformActionWithLock(string resource, TimeSpan expirationTime, Action action)
        {
            // use RedLock library
            using (var redisLock = this.redisLockFactory.CreateLock(resource, expirationTime))
            {
                // ensure that lock is acquired
                if (!redisLock.IsAcquired)
                {
                    return false;
                }

                // perform action
                action();

                return true;
            }
        }

        /// <inheritdoc cref="IDisposable"/>
        public void Dispose()
        {
            // dispose ConnectionMultiplexer
            this.connection?.Dispose();

            // dispose RedLock factory
            this.redisLockFactory?.Dispose();
        }

        /// <summary>
        /// Get connection string.
        /// </summary>
        /// <returns>string.</returns>
        protected string GetConnectionString()
        {
            return this.config.RedisCachingConnectionString;
        }

        /// <summary>
        /// Get connection.
        /// </summary>
        /// <returns>ConnectionMultiplexer.</returns>
        protected ConnectionMultiplexer GetConnection()
        {
            if (this.connection != null && this.connection.IsConnected)
            {
                return this.connection;
            }

            lock (this.@lock)
            {
                if (this.connection != null && this.connection.IsConnected)
                {
                    return this.connection;
                }

                // Connection disconnected. Disposing connection...
                this.connection?.Dispose();

                // Creating new instance of Redis Connection
                this.connection = ConnectionMultiplexer.Connect(this.connectionString.Value);
            }

            return this.connection;
        }

        /// <summary>
        /// Create redisLock factory.
        /// </summary>
        /// <returns>RedLockFactory.</returns>
        protected RedLockFactory CreateRedisLockFactory()
        {
            // get RedLock endpoints
            var configurationOptions = ConfigurationOptions.Parse(this.connectionString.Value);
            var redLockEndPoints = this.GetEndPoints().Select(endPoint => new RedLockEndPoint
            {
                EndPoint = endPoint,
                Password = configurationOptions.Password,
                Ssl = configurationOptions.Ssl,
                RedisDatabase = configurationOptions.DefaultDatabase,
                ConfigCheckSeconds = configurationOptions.ConfigCheckSeconds,
                ConnectionTimeout = configurationOptions.ConnectTimeout,
                SyncTimeout = configurationOptions.SyncTimeout,
            }).ToList();

            // create RedLock factory to use RedLock distributed lock algorithm
            return RedLockFactory.Create(redLockEndPoints);
        }
    }
}
