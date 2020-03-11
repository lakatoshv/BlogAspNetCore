namespace Blog.Core.Infrastructure.Pagination
{
    using Blog.Core.Infrastructure.Pagination.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    public class SearchQuery<TEntity>
    {
        public SearchQuery()
        {
            Filters = new List<Expression<Func<TEntity, bool>>>();
            SortCriterias = new List<ISortCriteria<TEntity>>();
        }

        public List<Expression<Func<TEntity, bool>>> Filters { get; protected set; }

        public void AddFilter(Expression<Func<TEntity, Boolean>> filter)
        {
            Filters.Add(filter);
        }

        public List<ISortCriteria<TEntity>> SortCriterias
        {
            get;
            protected set;
        }

        public void AddSortCriteria(ISortCriteria<TEntity> sortCriteria)
        {
            SortCriterias.Add(sortCriteria);
        }

        public string IncludeProperties { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }
    }
}
