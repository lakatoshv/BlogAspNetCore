using Blog.Contracts.V1.Responses.UsersResponses;
using Blog.Core.Consts;
using Microsoft.AspNetCore.Identity;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Blog.Web.SwaggerExamples.Responses.UsersResponses
{
    /// <summary>
    /// Profile response example.
    /// </summary>
    /// <seealso cref="IExamplesProvider{ProfileResponse}" />
    public class ProfileResponseExample : IExamplesProvider<ProfileResponse>
    {
        /// <inheritdoc cref="IExamplesProvider{T}"/>
        public ProfileResponse GetExamples()
        {
            return new ProfileResponse
            {
                UserId = Guid.NewGuid().ToString(),

                User = new ApplicationUserResponse
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
                },

                About = SwaggerExamplesConsts.ProfileResponseExample.About,
                ProfileImg = SwaggerExamplesConsts.ProfileResponseExample.ProfileImg,
            };
        }
    }
}
