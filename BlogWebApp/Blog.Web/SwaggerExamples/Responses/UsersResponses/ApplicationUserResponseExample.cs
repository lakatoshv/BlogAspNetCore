using Blog.Contracts.V1.Responses.UsersResponses;
using Blog.Core.Consts;
using Microsoft.AspNetCore.Identity;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Blog.Web.SwaggerExamples.Responses.UsersResponses
{
    /// <summary>
    /// Application user response example.
    /// </summary>
    /// <seealso cref="IExamplesProvider{ApplicationUserResponse}" />
    public class ApplicationUserResponseExample : IExamplesProvider<ApplicationUserResponse>
    {
        /// <inheritdoc cref="IExamplesProvider{T}"/>
        public ApplicationUserResponse GetExamples()
        {
            return new ApplicationUserResponse
            {
                FirstName = SwaggerExamplesConsts.AccountExample.FirstName,
                LastName = SwaggerExamplesConsts.AccountExample.LastName,
                Email = SwaggerExamplesConsts.AccountExample.Email,
                EmailConfirmed = true,
                Roles = new List<IdentityUserRole<string>>
                {
                    new IdentityUserRole<string>
                    {
                        RoleId = Guid.NewGuid().ToString(),
                    }
                }
            };
        }
    }
}
