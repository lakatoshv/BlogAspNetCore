namespace Blog.Services.Core.Caching.Interfaces
{
    using System;
    using System.Net;
    using StackExchange.Redis;

    public interface IRedisConnectionWrapper : IDisposable
    {
        IDatabase GetDatabase(int? db = null);

        IServer GetServer(EndPoint endPoint);

        EndPoint[] GetEndPoints();

        void FlushDatabase(int? db = null);
    }
}
