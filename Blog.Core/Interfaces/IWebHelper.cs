namespace Blog.Core.Interfaces
{
    using Microsoft.AspNetCore.Http;
    public interface IWebHelper
    {
        string GetUrlReferrer();

        string GetCurrentIpAddress();

        string GetThisPageUrl(bool includeQueryString, bool? useSsl = null, bool lowercaseUrl = false);

        bool IsCurrentConnectionSecured();

        string GetStoreHost(bool useSsl);

        string GetStoreLocation(bool? useSsl = null);

        bool IsStaticResource();

        string ModifyQueryString(string url, string key, params string[] values);

        string RemoveQueryString(string url, string key, string value = null);

        T QueryString<T>(string name);

        void RestartAppDomain(bool makeRedirect = false);

        bool IsRequestBeingRedirected { get; }

        bool IsPostBeingDone { get; set; }

        string CurrentRequestProtocol { get; }

        bool IsLocalRequest(HttpRequest req);

        string GetRawUrl(HttpRequest request);
    }
}
