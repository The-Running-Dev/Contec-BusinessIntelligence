using System;
using System.Data;

namespace Contec.Framework.Data
{
    public class BaseRepository
    {
        protected readonly DateTime SqlMinValue = new DateTime(1900, 1, 1);

        public BaseRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        protected DateTime GetValidSqlDateTime(DateTime original)
        {
            return original < SqlMinValue ? SqlMinValue : original;
        }

        protected T Query<T>(Func<IDbConnection, T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("Func");
            }

            using (var connection = _connectionFactory.Create())
            {
                try
                {
                    return func(connection);
                }
                catch (Exception e)
                {
                    //_logService.Error(this, "Error While Querying Database.", e);
                }
                finally
                {
                    connection.Close();
                }
            }

            return default(T);
        }

        protected T Execute<T>(Func<IDbConnection, T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("Func");
            }

            using (var connection = _connectionFactory.Create())
            {
                try
                {
                    return func(connection);
                }
                catch (Exception e)
                {
                    //_logService.Error(this, "Error While Executing Command.", e);

                    throw;
                }
            }
        }

        protected void Execute(Action<IDbConnection> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("Action");
            }

            using (var connection = _connectionFactory.Create())
            {
                try
                {
                    action(connection);
                }
                catch (Exception e)
                {
                    //_logService.Error(this, "Error While Executing Command.", e);

                    throw;
                }
            }
        }

        protected T ExecuteTransaction<T>(Func<IDbConnection, IDbTransaction, T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("Func");
            }

            using (var connection = _connectionFactory.Create())

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var result = func(connection, transaction);

                    transaction.Commit();

                    return result;
                }
                catch (Exception)
                {
                    try
                    {
                        // Attempt to roll back the transaction
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        //_logService.Warn(GetType(), $"Rollback Exception Type: {ex2.GetType()},  Message: {ex2.Message}");
                    }

                    throw;
                }
            }
        }

        protected void ExecuteTransaction(Action<IDbConnection, IDbTransaction> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("Action");
            }

            using (var connection = _connectionFactory.Create())

            using (var transaction = connection.BeginTransaction())
            {

                try
                {
                    action(connection, transaction);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    try
                    {
                        // Attempt to roll back the transaction
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        // _logService.Warn(GetType(), $"Rollback Exception Type: {ex2.GetType()},  Message: {ex2.Message}");
                    }

                    throw;
                }
            }
        }

        private readonly IConnectionFactory _connectionFactory;
    }
}