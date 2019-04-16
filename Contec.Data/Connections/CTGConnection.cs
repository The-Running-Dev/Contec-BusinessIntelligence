using JetBrains.Annotations;

using Contec.Framework.Configuration;

namespace Contec.Data.Connections
{
    public interface ICTGConnection
    {
    }

    [UsedImplicitly]
    public class CTGConnection : MySqlConnectionFactory, ICTGConnection
    {
        public CTGConnection(IConfiguration configuration): base(configuration)
        {
        }
    }
}