// <copyright file="EmailExtensionService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.CommonServices.EmailServices;

using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Core.Emails;
using Blog.Services.Core.Email.Templates;
using Interfaces;

/// <summary>
/// Email extension service.
/// </summary>
public class EmailExtensionService : IEmailExtensionService
{
    /// <summary>
    /// Email service.
    /// </summary>
    private readonly IEmailService emailService;

    /// <summary>
    /// Options.
    /// </summary>
    private readonly EmailExtensionOptions options;

    /// <summary>
    /// Email template provider.
    /// </summary>
    private readonly IEmailTemplateProvider emailTemplateProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailExtensionService"/> class.
    /// </summary>
    /// <param name="emailService">emailService.</param>
    /// <param name="options">options.</param>
    /// <param name="emailTemplateProvider">emailTemplateProvider.</param>
    public EmailExtensionService(IEmailService emailService, IOptions<EmailExtensionOptions> options, IEmailTemplateProvider emailTemplateProvider)
    {
        this.emailService = emailService;
        this.options = options.Value;
        this.emailTemplateProvider = emailTemplateProvider;
    }

    /// <inheritdoc cref="IEmailExtensionService"/>
    public async Task SendPasswordResetEmailAsync(string email, string token)
    {
        var body = this.emailTemplateProvider.ResolveBody(TemplateTypes.PasswordRestore, new { token, this.options.BaseUrl, email });
        await this.emailService.SendAsync(body, "Reset Password", this.options.From, email);
    }

    /// <inheritdoc cref="IEmailExtensionService"/>
    public async Task SendVerificationEmailAsync(string email, string token)
    {
        var body = this.emailTemplateProvider.ResolveBody(TemplateTypes.EmailVerification, new { token, this.options.BaseUrl, email });
        await this.emailService.SendAsync(body, "Verify Email", this.options.From, email);
    }
}