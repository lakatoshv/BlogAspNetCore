namespace Blog.Services.EmailServices.Interfaces
{
    using System.Threading.Tasks;

    public interface IEmailService
    {
        void Send(Blog.Core.Emails.Email email);
        void Send(string body, string subject, string from, string to);
        Task SendAsync(Blog.Core.Emails.Email email);
        Task SendAsync(string body, string subject, string from, string to);
    }
}
