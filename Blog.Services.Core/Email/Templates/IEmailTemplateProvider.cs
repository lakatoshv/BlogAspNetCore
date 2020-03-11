namespace Blog.Services.Core.Email.Templates
{
    public interface IEmailTemplateProvider
    {
        string ResolveBody<T>(TemplateTypes templateType, T model);
    }
}
