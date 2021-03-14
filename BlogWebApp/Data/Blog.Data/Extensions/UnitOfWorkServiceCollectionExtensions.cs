namespace Blog.Data.Extensions
{
    using Blog.Core;
    using Blog.Data.Repository;
    using Blog.Data.RepositoryFactory;
    using Blog.Data.UnitOfWork;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Unit of work service collection extensions.
    /// </summary>
    public static class UnitOfWorkServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the unit of work.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="services">The services.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddUnitOfWork<TContext>(
          this IServiceCollection services)
          where TContext : DbContext
        {
            services.AddScoped<IRepositoryFactory, UnitOfWork<TContext>>();
            services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
            services.AddScoped<IUnitOfWork<TContext>, UnitOfWork<TContext>>();
            return services;
        }

        /// <summary>
        /// Adds the unit of work.
        /// </summary>
        /// <typeparam name="TContext1">The type of the context1.</typeparam>
        /// <typeparam name="TContext2">The type of the context2.</typeparam>
        /// <param name="services">The services.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddUnitOfWork<TContext1, TContext2>(
          this IServiceCollection services)
          where TContext1 : DbContext
          where TContext2 : DbContext
        {
            services.AddScoped<IUnitOfWork<TContext1>, UnitOfWork<TContext1>>();
            services.AddScoped<IUnitOfWork<TContext2>, UnitOfWork<TContext2>>();
            return services;
        }

        /// <summary>
        /// Adds the unit of work.
        /// </summary>
        /// <typeparam name="TContext1">The type of the context1.</typeparam>
        /// <typeparam name="TContext2">The type of the context2.</typeparam>
        /// <typeparam name="TContext3">The type of the context3.</typeparam>
        /// <param name="services">The services.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddUnitOfWork<TContext1, TContext2, TContext3>(
          this IServiceCollection services)
          where TContext1 : DbContext
          where TContext2 : DbContext
          where TContext3 : DbContext
        {
            services.AddScoped<IUnitOfWork<TContext1>, UnitOfWork<TContext1>>();
            services.AddScoped<IUnitOfWork<TContext2>, UnitOfWork<TContext2>>();
            services.AddScoped<IUnitOfWork<TContext3>, UnitOfWork<TContext3>>();
            return services;
        }

        /// <summary>
        /// Adds the unit of work.
        /// </summary>
        /// <typeparam name="TContext1">The type of the context1.</typeparam>
        /// <typeparam name="TContext2">The type of the context2.</typeparam>
        /// <typeparam name="TContext3">The type of the context3.</typeparam>
        /// <typeparam name="TContext4">The type of the context4.</typeparam>
        /// <param name="services">The services.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddUnitOfWork<TContext1, TContext2, TContext3, TContext4>(
          this IServiceCollection services)
          where TContext1 : DbContext
          where TContext2 : DbContext
          where TContext3 : DbContext
          where TContext4 : DbContext
        {
            services.AddScoped<IUnitOfWork<TContext1>, UnitOfWork<TContext1>>();
            services.AddScoped<IUnitOfWork<TContext2>, UnitOfWork<TContext2>>();
            services.AddScoped<IUnitOfWork<TContext3>, UnitOfWork<TContext3>>();
            services.AddScoped<IUnitOfWork<TContext4>, UnitOfWork<TContext4>>();
            return services;
        }

        /// <summary>
        /// Adds the custom repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TRepository">The type of the repository.</typeparam>
        /// <param name="services">The services.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddCustomRepository<TEntity, TRepository>(
          this IServiceCollection services)
          where TEntity : class
          where TRepository : class, IRepository<IEntity>
        {
            services.AddScoped<IRepository<IEntity>, TRepository>();
            return services;
        }
    }
}