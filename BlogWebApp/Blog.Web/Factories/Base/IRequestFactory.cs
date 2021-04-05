namespace Blog.Web.Factories.Base
{
    using Blog.Contracts.V1.Requests.Interfaces;
    using Blog.Core;

    /// <summary>
    /// Request factory interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TCreateRequest">The type of the create request.</typeparam>
    /// <typeparam name="TUpdateRequest">The type of the update request.</typeparam>
    public interface IRequestFactory<in T, TEntity, out TCreateRequest, out TUpdateRequest>
        where TCreateRequest : IRequest, new()
        where TUpdateRequest : IRequest, new()
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Returns Request for entity creation.
        /// </summary>
        /// <returns>TCreateRequest.</returns>
        TCreateRequest GenerateForCreate();

        /// <summary>
        /// Returns Request for entity editing.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TUpdateRequest.</returns>
        TUpdateRequest GenerateForUpdate(T id);
    }
}