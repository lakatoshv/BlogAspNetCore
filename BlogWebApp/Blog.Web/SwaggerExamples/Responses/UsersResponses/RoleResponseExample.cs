namespace Blog.Web.SwaggerExamples.Responses.UsersResponses;

using System;
using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Responses.UsersResponses;
using Core.Consts;

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