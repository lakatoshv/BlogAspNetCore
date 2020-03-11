
namespace Blog.Data.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Blog.Core;
    using Blog.Core.Infrastructure.Pagination;
    using Blog.Core.TableFilters;

    public interface IRepository<T> where T : IEntity
    {
        #region Methods

        IQueryable<T> GetAll();

        IQueryable<T> GetAll(Expression<Func<T, bool>> expression);

        T GetById(object id);

        Task<T> GetByIdAsync(object id);

        Task<PagedListResult<T>> SearchAsync(SearchQuery<T> searchQuery);

        Task<PagedListResult<T>> SearchBySquenceAsync(SearchQuery<T> searchQuery, IQueryable<T> sequence);

        SearchQuery<T> GenerateQuery(TableFilter tableFilter, string includeProperties = null);

        void Insert(T entity);

        void Insert(IEnumerable<T> entities);

        void Update(T entity);

        void Update(IEnumerable<T> entities);

        void Delete(T entity);

        void Delete(IEnumerable<T> entities);

        bool Any(Expression<Func<T, bool>> expression);

        T FirstOrDefault(Expression<Func<T, bool>> expression);

        T LastOrDefault(Expression<Func<T, bool>> expression);
        #endregion

        #region Properties

        IQueryable<T> Table { get; }

        IQueryable<T> TableNoTracking { get; }

        #endregion
    }
}
