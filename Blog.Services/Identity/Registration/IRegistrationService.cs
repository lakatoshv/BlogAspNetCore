namespace Blog.Services.Identity.Registration
{
    using Microsoft.AspNetCore.Identity;
    using Blog.Data.Models;
    using System.Threading.Tasks;

    public interface IRegistrationService
    {
        // TODO: Refactor, no need to return Identity Result. Just a bool (.IsSuccess) should be sufficient.
        // This will ubind us from Microsoft.AspNetCore.Identity
        IdentityResult Register(ApplicationUser user, string password);
        Task<IdentityResult> RegisterAsync(ApplicationUser user, string password);
    }
}
