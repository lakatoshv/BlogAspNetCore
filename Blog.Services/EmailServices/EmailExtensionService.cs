// <copyright file="EmailExtensionService.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Services.EmailServices
{
    using System.Threading.Tasks;
    using Blog.Core.Emails;
    using Core.Email.Templates;
    using Interfaces;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Email extension service.
    /// </summary>
    public class EmailExtensionService : IEmailExtensionService
    {
        /// <summary>
        /// Email service.
        /// </summary>
        private readonly IEmailService _emailService;

        /// <summary>
        /// Options.
        /// </summary>
        private readonly EmailExtensionOptions _options;

        /// <summary>
        /// Email template provider.
        /// </summary>
        private readonly IEmailTemplateProvider _emailTemplateProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailExtensionService"/> class.
        /// </summary>
        /// <param name="emailService">emailService.</param>
        /// <param name="options">options.</param>
        /// <param name="emailTemplateProvider">emailTemplateProvider.</param>
        public EmailExtensionService(IEmailService emailService, IOptions<EmailExtensionOptions> options, IEmailTemplateProvider emailTemplateProvider)
        {
            this._emailService = emailService;
            this._options = options.Value;
            this._emailTemplateProvider = emailTemplateProvider;
        }

        /// <inheritdoc cref="IEmailExtensionService"/>
        public async Task SendPasswordResetEmailAsync(string email, string token)
        {
            var body = this._emailTemplateProvider.ResolveBody(TemplateTypes.PasswordRestore, new { token, this._options.BaseUrl, email });
            await this._emailService.SendAsync(body, "Reset Password", this._options.From, email);
        }

        /// <inheritdoc cref="IEmailExtensionService"/>
        public async Task SendVerificationEmailAsync(string email, string token)
        {
            var body = this._emailTemplateProvider.ResolveBody(TemplateTypes.EmailVerification, new { token, this._options.BaseUrl, email });
            await this._emailService.SendAsync(body, "Verify Email", this._options.From, email);
        }
    }
}
