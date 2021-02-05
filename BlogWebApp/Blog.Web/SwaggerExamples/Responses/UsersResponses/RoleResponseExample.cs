using Blog.Contracts.V1.Responses.UsersResponses;
using Blog.Core.Consts;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Blog.Web.SwaggerExamples.Responses.UsersResponses
{
    /// <summary>
    /// Role response example.
    /// </summary>
    /// <seealso cref="IExamplesProvider{PostTagRelationsResponse}" />
    public class RoleResponseExample : IExamplesProvider<RoleResponse>
    {
        /// <inheritdoc cref="IExamplesProvider{T}"/>
        public RoleResponse GetExamples()
        {
            return new RoleResponse
            {
                Id = Guid.NewGuid().ToString(),
                Name = Consts.Roles.User,
            };
        }
    }
}
