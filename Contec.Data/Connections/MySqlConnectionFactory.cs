using System.Data;

using MySql.Data.MySqlClient;
using Contec.Framework.Data;
using Contec.Framework.Configuration;

namespace Contec.Data.Connections
{
    public class MySqlConnectionFactory : IConnectionFactory
    {
        public MySqlConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionStringName = GetType().Name;

            InitializePropeties();
            _configuration.OnReloadComplete += InitializePropeties;
        }

        public IDbConnection Create()
        {
            var connection = new MySqlConnection(_connectionString);

            connection.Open();

            return connection;
        }

        private void InitializePropeties()
        {
            _connectionString = _configuration.GetGroupSetting("ConnectionStrings", _connectionStringName);
        }

        private readonly IConfiguration _configuration;
        private readonly string _connectionStringName;
        private string _connectionString;
    }
}