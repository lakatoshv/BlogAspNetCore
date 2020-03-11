namespace Blog.Services.GeneralService
{
    using Blog.Core.Infrastructure.Pagination;
    using Blog.Core.TableFilters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IGeneralService<T>
    {
        T Find(object id);
        Task<T> FindAsync(object id);

        void Insert(T entity);

        void Insert(IEnumerable<T> entities);

        void Update(T entity);

        void Update(IEnumerable<T> entities);

        void Delete(T entity);

        void Delete(IEnumerable<T> entities);

        Task<PagedListResult<T>> SearchAsync(SearchQuery<T> searchQuery);

        Task<PagedListResult<T>> SearchBySquenceAsync(SearchQuery<T> searchQuery,
            IQueryable<T> sequence);

        SearchQuery<T> GenerateQuery(TableFilter tableFilter, string includeProperties = null);

        ICollection<T> GetAll();

        ICollection<T> GetAll(Expression<Func<T, bool>> expression);

        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> expression);

        bool Any(Expression<Func<T, bool>> expression);

        T FirstOrDefault(Expression<Func<T, bool>> expression);

        T LastOrDefault(Expression<Func<T, bool>> expression);
    }
}
