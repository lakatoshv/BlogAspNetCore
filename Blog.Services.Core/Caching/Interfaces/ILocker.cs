namespace Blog.Services.Core.Caching.Interfaces
{
    using System;

    public interface ILocker
    {
        bool PerformActionWithLock(string resource, TimeSpan expirationTime, Action action);
    }
}
