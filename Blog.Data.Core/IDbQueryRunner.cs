namespace Blog.Data.Core
{
    using System;

    public interface IDbQueryRunner : IDisposable
    {
        void RunQuery(string query, params object[] parameters);
    }
}
