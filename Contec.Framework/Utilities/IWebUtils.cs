namespace Contec.Framework.Utilities
{
    public interface IWebUtils
    {
        string Hostname(string requestUri);

        T QueryString<T>(string key, bool validateInput = true);

        T PostValue<T>(string key);

        T HeaderValue<T>(string key);

        bool IsValidUrl(string url);
    }
}