namespace Blog.Web.Factories.Base
{
    using Blog.Contracts.V1.Requests.Interfaces;
    using Blog.Core;

    /// <summary>
    /// Request factory with sample.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TSampleRequest">The type of the sample request.</typeparam>
    /// <typeparam name="TCreateRequest">The type of the create request.</typeparam>
    /// <typeparam name="TUpdateRequest">The type of the update request.</typeparam>
    /// <seealso>
    ///     <cref>RequestFactory{T, TEntity, TCreateRequest, TUpdateRequest}</cref>
    /// </seealso>
    public abstract class RequestFactoryWithSample<T, TEntity, TSampleRequest, TCreateRequest, TUpdateRequest> : RequestFactory<T, TEntity, TCreateRequest, TUpdateRequest>
        where TCreateRequest : IRequest, new()
        where TSampleRequest : IRequest, new()
        where TUpdateRequest : IRequest, new()
        where TEntity : class, IEntity
    {

        /// <summary>
        /// Generate for sample request.
        /// </summary>
        /// <returns>TSearchParametersRequest.</returns>
        public abstract TSampleRequest GenerateForSampleRequest();
    }
}