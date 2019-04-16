using JetBrains.Annotations;

using Contec.Framework.Configuration;

namespace Contec.Data.Connections
{
    public interface IWebAuthConnection
    {
    }

    [UsedImplicitly]
    public class WebAuthConnection : MySqlConnectionFactory, IWebAuthConnection
    {
        public WebAuthConnection(IConfiguration configuration) : base(configuration)
        {
        }
    }
}