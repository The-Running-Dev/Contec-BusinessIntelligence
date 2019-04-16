using System.Linq;
using System.Web.Http;
using System.Net.Http.Formatting;

using API.Formatters;

namespace API
{
    public class FormattersConfig
    {
        public static void Register(MediaTypeFormatterCollection formatters)
        {
            // Remove the XML media type formatter
            formatters.XmlFormatter.SupportedMediaTypes.Remove(
                formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml"));

            // Configure the JSON media type formatter to ignore serializable attribute
            ((Newtonsoft.Json.Serialization.DefaultContractResolver)(GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver)).IgnoreSerializableAttribute = true;

            // Add text/ csv support
            formatters.Add(new CsvMediaTypeFormatter(false));

            // Add application/octet-stream support
            formatters.Add(new BinaryMediaTypeFormatter());
        }
    }
}