using System.Data;

namespace Contec.Framework.Data
{
    public interface IConnectionFactory
    {
        IDbConnection Create();
    }
}