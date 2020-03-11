namespace Blog.Services.EmailServices
{
    using System.Threading.Tasks;
    using Blog.Core.Emails;
    using Blog.Services.Core.Email.Templates;
    using Blog.Services.EmailServices.Interfaces;
    using Microsoft.Extensions.Options;

    public class EmailExtensionService : IEmailExtensionService
    {
        private readonly IEmailService _emailService;
        private readonly EmailExtensionOptions _options;
        private readonly IEmailTemplateProvider _emailTemplateProvider;

        public EmailExtensionService(IEmailService emailService, IOptions<EmailExtensionOptions> options, IEmailTemplateProvider emailTemplateProvider)
        {
            _emailService = emailService;
            _options = options.Value;
            _emailTemplateProvider = emailTemplateProvider;
        }

        public async Task SendPasswordResetEmailAsync(string email, string token)
        {
            var body = _emailTemplateProvider.ResolveBody(TemplateTypes.PasswordRestore, new { token, _options.BaseUrl, email });
            await _emailService.SendAsync(body, "Reset Password", _options.From, email);
        }

        public async Task SendVerificationEmailAsync(string email, string token)
        {
            var body = _emailTemplateProvider.ResolveBody(TemplateTypes.EmailVerification, new { token, _options.BaseUrl, email });
            await _emailService.SendAsync(body, "Verify Email", _options.From, email);
        }
    }
}
