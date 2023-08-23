namespace Blog.Web.SwaggerExamples.Requests.UsersRequests;

using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Requests.UsersRequests;
using Core.Consts;

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