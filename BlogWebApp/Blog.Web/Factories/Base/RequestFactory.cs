namespace Blog.Web.Factories.Base
{
    using Blog.Contracts.V1.Requests.Interfaces;
    using Blog.Core;

    /// <summary>
    /// Request factory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TCreateRequest">The type of the create request.</typeparam>
    /// <typeparam name="TUpdateRequest">The type of the update request.</typeparam>
    /// <seealso cref="IRequestFactory{T, TEntity, TCreateRequest, TUpdateRequest}" />
    public abstract class RequestFactory<T, TEntity, TCreateRequest, TUpdateRequest> : IRequestFactory<T, TEntity, TCreateRequest, TUpdateRequest>
        where TCreateRequest : IRequest, new()
        where TUpdateRequest : IRequest, new()
        where TEntity : class, IEntity
    {

        /// <inheritdoc />
        public abstract TCreateRequest GenerateForCreate();

        /// <inheritdoc />
        public abstract TUpdateRequest GenerateForUpdate(T id);
    }
}