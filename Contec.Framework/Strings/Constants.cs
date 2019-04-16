namespace Contec.Framework.Strings
{
    public class Constants
    {
        public const string ConvertUrlToLinkRegEx = "\\bhttps?://([-A-Z0-9+&@#/%?=~_|!:,.;]*[-A-Z0-9+&@#/%=~_|])";
        public const string DirectoryNameValidationRegEx = "^[a-zA-Z0-9\\-\\._]*$";
        public const string DirectoryNameReplaceRegEx = "[^a-zA-Z0-9\\-\\._]";

        public const string ConvertUrlToLinkRegExReplace = "<a target=\"_blank\" href=\"$0\">$1</a>";

        public const string LocalhostIpAddress = "127.0.0.1";
        public const string LocalHostname = "localhost";
        public const string HttpReferrer = "http_referer";
        public const string HttpEppn = "http_eppn";
        public const string ServerName = "server_name";
        public const string UserAgent = "http_user_agent";

        public const string ScriptName = "script_name";
        public const string UrlPartsRegEx = "\\b(?<protocol>[^:/?#]+)://(?<domain>[-A-Z0-9.]+)(?<app>/[-A-Z0-9+&@#%=~_|!:,.;\\s]*)(?<path>/[-A-Z0-9+&@#/%=~_|!':,.;\\s\\(\\)]*)?(?<parameters>\\?[A-Z0-9+&@#/%=~_|!:,.;\\-]*)?";
        public const string UrlPartsFileNameRegEx = "/([-A-Z0-9+&@#%=~_|!:,.;]*)$";
        public const string UrlPartsProtocol = "${protocol}";
        public const string UrlPartsHostName = "${domain}";
        public const string UrlPartsAppName = "${app}";
        public const string UrlPartsFileName = "${path}";

        public const string IDeviceUserAgentRegEx = "(iPad|iPod|iPhone)";
        public const string AndroidUserAgentRegEx = "android";

        public const string UrlPartsFullAppUrl = "${protocol}://${domain}${app}";
        public const string IpAddressRegEx = "(?:(?:25[0-5]|2[0-4]\\d|[01]\\d\\d|\\d?\\d)(?(?=\\.?\\d)\\.)){4}";
        public const string IpAddressWithRangeRegEx = "(?:(?:25[0-5]|2[0-4]\\d|[01]\\d\\d|\\d?\\d)(?(?=\\.?\\d)\\.)){4}(/([1-2]\\d|3[0-2]|\\d))";
        public const string DomainNameRegEx = "^([A-Za-z0-9]+([\\-A-Z-a-z0-9]*[A-Za-z0-9]+)?\\.){0,}([A-Za-z0-9]+([\\-A-Z-a-z0-9]*[A-Za-z0-9]+)?){1,63}(\\.[a-z0-9]{2,7})+$";
        public const string DomainNameOrIpAddressRegEx = "(^([A-Za-z0-9]+([\\-A-Z-a-z0-9]*[A-Za-z0-9]+)?\\.){0,}([A-Za-z0-9]+([\\-A-Z-a-z0-9]*[A-Za-z0-9]+)?){1,63}(\\.[a-z0-9]{2,7})+$)|(^(?:(?:25[0-5]|2[0-4]\\d|[01]\\d\\d|\\d?\\d)(?(?=\\.?\\d)\\.)){4}(/([1-2]\\d|3[0-2]|\\d))?$)";

        public const string RelativePathToLibraryJavaScriptFiles = "~/js/library/{0}";
        public const string RelativePathToAdminJavaScriptFiles = "~/js/admin/{0}";
        public const string RelativePathToContentJavaScriptFiles = "~/js/content/{0}";
        public const string EmptyJavaScriptTag = "<script type=\"text/javascript\"></script>";
        public const string JavaScriptTag = "<script type=\"text/javascript\" src=\"{0}?NoCaching={1}\"></script>";

        public const string JavaScriptInlineTag = "<script type=\"text/javascript\">{0}{1}{0}</script>";
        
        public const string SortDirectionAscending = "ASC";
        public const string SortDirectionDescending = "DESC";

        public const string TrueValues = "1,true";
        public const string FalseValues = "0,false";

        public const string NumbersAgoRegEx = "(one|two|three|four|five|six|seven|eight|nine|ten|" + "eleven|twelve|thirteen|fourteen|fifteen|sixteen|seventeen|eighteen|nineteen|twenty) ((Minute)|(Hour)|(Day)|(Week)|(Month))(s)? Ago";

        public const string NumbersAsStringRegEx = "one|two|three|four|five|six|seven|eight|nine|ten|" + "eleven|twelve|thirteen|fourteen|fifteen|sixteen|seventeen|eighteen|nineteen|twenty";
    }
}