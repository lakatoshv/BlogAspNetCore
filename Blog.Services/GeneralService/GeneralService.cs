namespace Blog.Services.GeneralService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Blog.Core.Infrastructure.Pagination;
    using Blog.Core.TableFilters;
    using Blog.Data.Core;
    using Blog.Data.Repository;
    using Microsoft.EntityFrameworkCore;

    public class GeneralService<T> : IGeneralService<T> where T : Entity
    {
        protected IRepository<T> Repository;

        protected IQueryable<T> Table => Repository.Table;
        protected IQueryable<T> TableNoTracking => Repository.TableNoTracking;
        protected static string GetMemberName<T, TValue>(Expression<Func<T, TValue>> memberAccess)
        {
            return ((MemberExpression)memberAccess.Body).Member.Name;
        }
        public GeneralService(IRepository<T> repository)
        {
            Repository = repository;
        }

        public T Find(object id)
        {
            return Repository.GetById(id);
        }
        public async Task<T> FindAsync(object id)
        {
            return await Repository.GetByIdAsync(id);
        }

        public void Insert(T entity)
        {
            Repository.Insert(entity);
        }

        public void Insert(IEnumerable<T> entities)
        {
            Repository.Insert(entities);
        }

        public void Update(T entity)
        {
            Repository.Update(entity);
        }

        public void Update(IEnumerable<T> entities)
        {
            Repository.Update(entities);
        }

        public void Delete(T entity)
        {
            Repository.Delete(entity);
        }

        public void Delete(IEnumerable<T> entities)
        {
            Repository.Delete(entities);
        }

        public async Task<PagedListResult<T>> SearchAsync(SearchQuery<T> searchQuery)
        {
            return await Repository.SearchAsync(searchQuery);
        }

        public async Task<PagedListResult<T>> SearchBySquenceAsync(SearchQuery<T> searchQuery,
            IQueryable<T> sequence)
        {
            return await Repository.SearchBySquenceAsync(searchQuery, sequence);
        }

        public ICollection<T> GetAll()
        {
            return Repository.GetAll().ToList();
        }

        public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> expression)
        {
            return await Repository.GetAll(expression).ToListAsync();
        }

        public ICollection<T> GetAll(Expression<Func<T, bool>> expression)
        {
            return Repository.GetAll(expression).ToList();
        }

        public bool Any(Expression<Func<T, bool>> expression)
        {
            return Repository.Any(expression);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return Repository.FirstOrDefault(expression);
        }

        public T LastOrDefault(Expression<Func<T, bool>> expression)
        {
            return Repository.LastOrDefault(expression);
        }

        public SearchQuery<T> GenerateQuery(TableFilter tableFilter, string includeProperties = null)
        {
            return Repository.GenerateQuery(tableFilter, includeProperties);
        }
    }
}
