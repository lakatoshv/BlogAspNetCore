namespace Blog.Services.EmailServices
{
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using System.Threading.Tasks;
    using Blog.Services.Core.Email.Smtp;
    using Microsoft.Extensions.Options;
    using Blog.Services.EmailServices.Interfaces;

    public class SmtpEmailService : IEmailService
    {
        private readonly SmtpClient _client;

        public SmtpEmailService(IOptions<SmtpOptions> smtpOptions)
        {
            var options = smtpOptions.Value;
            _client = new SmtpClient(options.Host, options.Port)
            {
                Credentials = new NetworkCredential(options.UserName, options.Password),
                EnableSsl = options.EnableSsl
            };
        }

        public void Send(Blog.Core.Emails.Email email)
        {
            var message = GetMailMessage(email.Body, email.Subject, email.From, email.To);
            _client.Send(message);
        }

        public void Send(string body, string subject, string from, string to)
        {
            // TODO: Investigate how smtp client from argument works
            var message = GetMailMessage(body, subject, from, to);
            _client.Send(message);

        }

        // TODO: Think of something to get rid of this warning: This async methods lacks await operators ...
        // TODO: Refactor: Consider creating some base MailMessage type and using it instead
        public async Task SendAsync(Blog.Core.Emails.Email email)
        {
            var message = GetMailMessage(email.Body, email.Subject, email.From, email.To);
            await _client.SendMailAsync(message);
        }

        public async Task SendAsync(string body, string subject, string from, string to)
        {
            var message = GetMailMessage(body, subject, from, to);
            await _client.SendMailAsync(message);
        }


        private MailMessage GetMailMessage(string body, string subject, string from, string to)
        {
            var message = new MailMessage(from, to)
            {
                Subject = subject,
                Body = body,

                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8
            };

            return message;
        }
    }
}
