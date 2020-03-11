﻿namespace Blog.Services.Core.Caching
{
    using System;
    using System.Linq;
    using System.Net;
    using Blog.Core.Configuration;
    using Blog.Services.Core.Caching.Interfaces;
    using RedLockNet.SERedis;
    using RedLockNet.SERedis.Configuration;
    using StackExchange.Redis;

    public class RedisConnectionWrapper : IRedisConnectionWrapper, ILocker
    {
        #region Fields

        private readonly BlogConfiguration _config;
        private readonly Lazy<string> _connectionString;
        private readonly object _lock = new object();
        private volatile ConnectionMultiplexer _connection;
        private volatile RedLockFactory _redisLockFactory;

        #endregion

        #region Ctor
        public RedisConnectionWrapper(BlogConfiguration config)
        {
            _config = config;
            _connectionString = new Lazy<string>(GetConnectionString);
            _redisLockFactory = CreateRedisLockFactory();
        }

        #endregion

        #region Utilities
        protected string GetConnectionString()
        {
            return _config.RedisCachingConnectionString;
        }

        protected ConnectionMultiplexer GetConnection()
        {
            if (_connection != null && _connection.IsConnected) return _connection;

            lock (_lock)
            {
                if (_connection != null && _connection.IsConnected) return _connection;

                //Connection disconnected. Disposing connection...
                _connection?.Dispose();

                //Creating new instance of Redis Connection
                _connection = ConnectionMultiplexer.Connect(_connectionString.Value);
            }

            return _connection;
        }

        protected RedLockFactory CreateRedisLockFactory()
        {
            //get RedLock endpoints
            var configurationOptions = ConfigurationOptions.Parse(_connectionString.Value);
            var redLockEndPoints = GetEndPoints().Select(endPoint => new RedLockEndPoint
            {
                EndPoint = endPoint,
                Password = configurationOptions.Password,
                Ssl = configurationOptions.Ssl,
                RedisDatabase = configurationOptions.DefaultDatabase,
                ConfigCheckSeconds = configurationOptions.ConfigCheckSeconds,
                ConnectionTimeout = configurationOptions.ConnectTimeout,
                SyncTimeout = configurationOptions.SyncTimeout
            }).ToList();

            //create RedLock factory to use RedLock distributed lock algorithm
            return RedLockFactory.Create(redLockEndPoints);
        }

        #endregion

        #region Methods
        public IDatabase GetDatabase(int? db = null)
        {
            return GetConnection().GetDatabase(db ?? -1);
        }

        EndPoint[] IRedisConnectionWrapper.GetEndPoints()
        {
            return GetEndPoints();
        }

        public IServer GetServer(EndPoint endPoint)
        {
            return GetConnection().GetServer(endPoint);
        }

        IDatabase IRedisConnectionWrapper.GetDatabase(int? db)
        {
            return GetDatabase(db);
        }

        public EndPoint[] GetEndPoints()
        {
            return GetConnection().GetEndPoints();
        }

        public void FlushDatabase(int? db = null)
        {
            var endPoints = GetEndPoints();

            foreach (var endPoint in endPoints)
            {
                GetServer(endPoint).FlushDatabase(db ?? -1);
            }
        }

        public bool PerformActionWithLock(string resource, TimeSpan expirationTime, Action action)
        {
            //use RedLock library
            using (var redisLock = _redisLockFactory.CreateLock(resource, expirationTime))
            {
                //ensure that lock is acquired
                if (!redisLock.IsAcquired)
                    return false;

                //perform action
                action();

                return true;
            }
        }

        public void Dispose()
        {
            //dispose ConnectionMultiplexer
            _connection?.Dispose();

            //dispose RedLock factory
            _redisLockFactory?.Dispose();
        }

        #endregion
    }
}
