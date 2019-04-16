namespace Contec.Framework.Data
{
    public class SqlDatabaseServer : IDatabaseServer
    {
        public bool IsConnectionValid(string connectionString)
        {
            var message = string.Empty;

            return true;
            //return DatabaseServer.IsConnectionValid(connectionString, ref message);
        }
    }
}