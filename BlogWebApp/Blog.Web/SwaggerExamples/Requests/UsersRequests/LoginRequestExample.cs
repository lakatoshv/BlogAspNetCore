using Blog.Contracts.V1.Requests.UsersRequests;
using Blog.Core.Consts;
using Swashbuckle.AspNetCore.Filters;

namespace Blog.Web.SwaggerExamples.Requests.UsersRequests
{
    /// <summary>
    /// Login request example.
    /// </summary>
    /// <seealso cref="IExamplesProvider{LoginRequest}" />
    public class LoginRequestExample : IExamplesProvider<LoginRequest>
    {
        /// <inheritdoc cref="IExamplesProvider{T}"/>
        public LoginRequest GetExamples()
        {
            return new LoginRequest
            {
                Email = SwaggerExamplesConsts.AccountExample.Email,
                Password = SwaggerExamplesConsts.AccountExample.Password,
            };
        }
    }
}
