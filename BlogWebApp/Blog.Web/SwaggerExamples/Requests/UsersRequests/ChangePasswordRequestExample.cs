using Blog.Contracts.V1.Requests.UsersRequests;
using Blog.Core.Consts;
using Swashbuckle.AspNetCore.Filters;

namespace Blog.Web.SwaggerExamples.Requests.UsersRequests
{
    /// <summary>
    /// Change password request example.
    /// </summary>
    /// <seealso cref="IExamplesProvider{ChangePasswordRequest}" />
    public class ChangePasswordRequestExample : IExamplesProvider<ChangePasswordRequest>
    {
        /// <inheritdoc cref="IExamplesProvider{T}"/>
        public ChangePasswordRequest GetExamples()
        {
            return new ChangePasswordRequest 
            { 
                OldPassword = SwaggerExamplesConsts.AccountExample.Password,
                NewPassword = SwaggerExamplesConsts.ChangePasswordRequestExample.NewPassword,
            };

        }
    }
}
