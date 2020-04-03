// <copyright file="RedisConnectionWrapper.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Caching
{
    using System;
    using System.Linq;
    using System.Net;
    using Blog.Core.Configuration;
    using Interfaces;
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
        private readonly BlogConfiguration _config;

        /// <summary>
        /// Lazy connection.
        /// </summary>
        private readonly Lazy<string> _connectionString;

        /// <summary>
        /// Lock object.
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// Connection multiplexer.
        /// </summary>
        private volatile ConnectionMultiplexer _connection;

        /// <summary>
        /// RedLock factory.
        /// </summary>
        private volatile RedLockFactory _redisLockFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisConnectionWrapper"/> class.
        /// </summary>
        /// <param name="config">config.</param>
        public RedisConnectionWrapper(BlogConfiguration config)
        {
            this._config = config;
            this._connectionString = new Lazy<string>(this.GetConnectionString);
            this._redisLockFactory = this.CreateRedisLockFactory();
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

        /// <inheritdoc/>
        EndPoint[] IRedisConnectionWrapper.GetEndPoints()
        {
            return this.GetEndPoints();
        }

        /// <inheritdoc/>
        public IServer GetServer(EndPoint endPoint)
        {
            return this.GetConnection().GetServer(endPoint);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public bool PerformActionWithLock(string resource, TimeSpan expirationTime, Action action)
        {
            // use RedLock library
            using (var redisLock = this._redisLockFactory.CreateLock(resource, expirationTime))
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

        /// <inheritdoc/>
        public void Dispose()
        {
            // dispose ConnectionMultiplexer
            this._connection?.Dispose();

            // dispose RedLock factory
            this._redisLockFactory?.Dispose();
        }

        /// <summary>
        /// Get connection string.
        /// </summary>
        /// <returns>string.</returns>
        protected string GetConnectionString()
        {
            return this._config.RedisCachingConnectionString;
        }

        /// <summary>
        /// Get connection.
        /// </summary>
        /// <returns>ConnectionMultiplexer.</returns>
        protected ConnectionMultiplexer GetConnection()
        {
            if (this._connection != null && this._connection.IsConnected)
            {
                return this._connection;
            }

            lock (this._lock)
            {
                if (this._connection != null && this._connection.IsConnected)
                {
                    return this._connection;
                }

                // Connection disconnected. Disposing connection...
                this._connection?.Dispose();

                // Creating new instance of Redis Connection
                this._connection = ConnectionMultiplexer.Connect(this._connectionString.Value);
            }

            return this._connection;
        }

        /// <summary>
        /// Create redisLock factory.
        /// </summary>
        /// <returns>RedLockFactory.</returns>
        protected RedLockFactory CreateRedisLockFactory()
        {
            // get RedLock endpoints
            var configurationOptions = ConfigurationOptions.Parse(this._connectionString.Value);
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
