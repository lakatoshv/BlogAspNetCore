// <copyright file="SmtpEmailService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.CommonServices.EmailServices;

using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Blog.Services.Core.Email.Smtp;
using Interfaces;

/// <summary>
/// Smtp email service.
/// </summary>
public class SmtpEmailService : IEmailService
{
    private readonly SmtpClient client;

    /// <summary>
    /// Initializes a new instance of the <see cref="SmtpEmailService"/> class.
    /// </summary>
    /// <param name="smtpOptions">smtpOptions.</param>
    public SmtpEmailService(IOptions<SmtpOptions> smtpOptions)
    {
        var options = smtpOptions.Value;
        this.client = new SmtpClient(options.Host, options.Port)
        {
            Credentials = new NetworkCredential(options.UserName, options.Password),
            EnableSsl = options.EnableSsl,
        };
    }

    /// <inheritdoc cref="IEmailService"/>
    public void Send(Blog.Core.Emails.Email email)
    {
        var message = GetMailMessage(email.Body, email.Subject, email.From, email.To);
        this.client.Send(message);
    }

    /// <inheritdoc cref="IEmailService"/>
    public void Send(string body, string subject, string from, string to)
    {
        // TODO: Investigate how smtp client from argument works
        var message = GetMailMessage(body, subject, from, to);
        this.client.Send(message);
    }

    // TODO: Think of something to get rid of this warning: This async methods lacks await operators ...
    // TODO: Refactor: Consider creating some base MailMessage type and using it instead

    /// <inheritdoc cref="IEmailService"/>
    public async Task SendAsync(Blog.Core.Emails.Email email)
    {
        var message = GetMailMessage(email.Body, email.Subject, email.From, email.To);
        await this.client.SendMailAsync(message);
    }

    /// <inheritdoc cref="IEmailService"/>
    public async Task SendAsync(string body, string subject, string from, string to)
    {
        var message = GetMailMessage(body, subject, from, to);
        await this.client.SendMailAsync(message);
    }

    /// <summary>
    /// Get mail message.
    /// </summary>
    /// <param name="body">body.</param>
    /// <param name="subject">subject.</param>
    /// <param name="from">from.</param>
    /// <param name="to">to.</param>
    /// <returns>MailMessage.</returns>
    private static MailMessage GetMailMessage(string body, string subject, string from, string to)
    {
        var message = new MailMessage(from, to)
        {
            Subject = subject,
            Body = body,

            IsBodyHtml = true,
            BodyEncoding = Encoding.UTF8,
        };

        return message;
    }
}