using System.Web;

using StructureMap;

using Contec.Framework.Data;
using Contec.Framework.Logging;
using Contec.Framework.Utilities;
using Contec.Framework.Configuration;
using Contec.Framework.Serialization;

namespace Contec.Bootstrapper.Shared
{
    public class FrameworkRegistry : Registry
    {
        public FrameworkRegistry()
        {
            For<IDatabaseServer>().Use<SqlDatabaseServer>();
            For<ISerializer>().Use<JsonSerializer>();
            For<IJsonSerializer>().Use<JsonSerializer>();
            For<IXmlSerializer>().Use<XmlSerializer>();
            For(typeof(IConfiguration<>)).Singleton().Use(typeof(ContecConfiguration<>));
            For<IConfiguration>().Singleton().Use<ContecConfiguration>();
            For<ILogService>().Use<Log4NetLogService>().Ctor<string>().Is(string.Empty);
            For<HttpContextBase>().Use(() => new HttpContextWrapper(HttpContext.Current));
            For<IWebUtils>().Use<WebUtils>();
        }
    }
}