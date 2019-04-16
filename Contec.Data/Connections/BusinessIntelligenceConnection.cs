using JetBrains.Annotations;

using Contec.Framework.Configuration;

namespace Contec.Data.Connections
{
    public interface IBusinessIntelligenceConnection
    {
    }

    [UsedImplicitly]
    public class BusinessIntelligenceConnection : MySqlConnectionFactory, IBusinessIntelligenceConnection
    {
        public BusinessIntelligenceConnection(IConfiguration configuration) : base(configuration)
        {
        }
    }
}