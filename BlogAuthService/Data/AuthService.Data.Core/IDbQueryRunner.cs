namespace AuthService.Data.Core
{
    using System;

    /// <summary>
    /// Db query runner interface.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public interface IDbQueryRunner : IDisposable
    {
        /// <summary>
        /// Runs the query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        void RunQuery(string query, params object[] parameters);
    }
}
