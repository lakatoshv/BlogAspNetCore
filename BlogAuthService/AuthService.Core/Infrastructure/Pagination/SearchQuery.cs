namespace AuthService.Core.Infrastructure.Pagination
{
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Search query.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class SearchQuery<TEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchQuery{TEntity}"/> class.
        /// </summary>
        public SearchQuery()
        {
            Filters = new List<Expression<Func<TEntity, bool>>>();
            SortCriterias = new List<ISortCriteria<TEntity>>();
        }

        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>
        /// The filters.
        /// </value>
        public List<Expression<Func<TEntity, bool>>> Filters { get; protected set; }

        /// <summary>
        /// Adds the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public void AddFilter(Expression<Func<TEntity, Boolean>> filter)
        {
            Filters.Add(filter);
        }

        /// <summary>
        /// Gets or sets the sort criterias.
        /// </summary>
        /// <value>
        /// The sort criterias.
        /// </value>
        public List<ISortCriteria<TEntity>> SortCriterias
        {
            get;
            protected set;
        }

        /// <summary>
        /// Adds the sort criteria.
        /// </summary>
        /// <param name="sortCriteria">The sort criteria.</param>
        public void AddSortCriteria(ISortCriteria<TEntity> sortCriteria)
        {
            SortCriterias.Add(sortCriteria);
        }

        /// <summary>
        /// Gets or sets the include properties.
        /// </summary>
        /// <value>
        /// The include properties.
        /// </value>
        public string IncludeProperties { get; set; }

        /// <summary>
        /// Gets or sets the skip.
        /// </summary>
        /// <value>
        /// The skip.
        /// </value>
        public int Skip { get; set; }

        /// <summary>
        /// Gets or sets the take.
        /// </summary>
        /// <value>
        /// The take.
        /// </value>
        public int Take { get; set; }
    }
}
