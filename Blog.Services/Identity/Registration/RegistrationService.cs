namespace Blog.Services.Identity.Registration
{
    using Microsoft.AspNetCore.Identity;
    using Blog.Data.Models;
    using Blog.Services.EmailServices.Interfaces;
    using System;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Blog.Services.Identity.User;

    public class RegistrationService : IRegistrationService
    {
        private readonly IUserService _userService;
        private readonly IEmailExtensionService _emailExtensionService;

        // TODO: Investigate if it is possible to abstracted out userrManager and Application context should to interfaces
        public RegistrationService(IUserService userService, IEmailExtensionService emailExtensionService)
        {
            _userService = userService;
            _emailExtensionService = emailExtensionService;
        }

        public IdentityResult Register(ApplicationUser user, string password)
        {
            // TODO: Either implement or remove
            throw new NotImplementedException();
        }

        // TODO: Get rid of IdentityResult, introduce own IServiceResult
        public async Task<IdentityResult> RegisterAsync(ApplicationUser user, string password)
        {
            user.UserName = Regex.Replace(user.UserName, @"\s+", " ").Trim();

            var result = await _userService.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var token = await _userService.GetEmailVerificationTokenAsync(user.UserName);
                //await _emailExtensionService.SendVerificationEmailAsync(user.UserName, token);
            }

            return result;
        }
    }
}
