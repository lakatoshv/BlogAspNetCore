namespace Blog.Web.Factories.Base
{
    using Blog.Contracts.V1.Requests.Interfaces;
    using Blog.Core;

    /// <summary>
    /// Request factory with search parameters.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TSearchParametersRequest">The type of the search parameters request.</typeparam>
    /// <typeparam name="TCreateRequest">The type of the create request.</typeparam>
    /// <typeparam name="TUpdateRequest">The type of the update request.</typeparam>
    /// <seealso cref="RequestFactory{T, TEntity, TCreateRequest, TUpdateRequest}" />
    public abstract class RequestFactoryWithSearchParameters<T, TEntity, TSearchParametersRequest, TCreateRequest, TUpdateRequest> : RequestFactory<T, TEntity, TCreateRequest, TUpdateRequest>
        where TCreateRequest : IRequest, new()
        where TSearchParametersRequest : IRequest, new()
        where TUpdateRequest : IRequest, new()
        where TEntity : class, IEntity
    {

        /// <summary>
        /// Generates for search parameters request.
        /// </summary>
        /// <returns>TSearchParametersRequest.</returns>
        public abstract TSearchParametersRequest GenerateForSearchParametersRequest();
    }
}