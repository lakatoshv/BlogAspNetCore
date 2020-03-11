namespace Blog.Services.EmailServices.Interfaces
{
    using System.Threading.Tasks;

    public interface IEmailExtensionService
    {
        Task SendVerificationEmailAsync(string email, string token);
        Task SendPasswordResetEmailAsync(string email, string token);
    }
}
