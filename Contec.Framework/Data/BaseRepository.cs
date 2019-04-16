using System;
using System.Collections.Generic;

using Dapper;

using Contec.Framework.Models;
using Contec.Framework.Logging;

namespace Contec.Framework.Data
{
    public class BaseRepository
    {
        /// <summary>
        ///  Constructs a new instance of the BaseRepository
        /// </summary>
        /// <param name="connectionFactory">The factory that will create the DB connection</param>
        /// <param name="logService"></param>
        public BaseRepository(IConnectionFactory connectionFactory, ILogService logService)
        {
            _connectionFactory = connectionFactory;
            _logService = logService;
        }

        public virtual DbResult<T> Single<T>(string sql, object param)
        {
            var dbResult = new DbResult<T>();

            try
            {
                using (var connection = _connectionFactory.Create())
                {
                    dbResult.Result = connection.QuerySingleOrDefault<T>(sql, param);
                }
            }
            catch (Exception ex)
            {
                dbResult.IsError = true;
                dbResult.Message = ex.InnerException?.Message ?? ex.Message;

                _logService.Error(this, "Error Executing Single", ex.InnerException ?? ex);
            }

            return dbResult;
        }

        public virtual DbResult<IEnumerable<T>> Query<T>(string sql, object param)
        {
            var dbResult = new DbResult<IEnumerable<T>>();

            try
            {
                using (var connection = _connectionFactory.Create())
                {
                    dbResult.Result = connection.Query<T>(sql, param);
                }
            }
            catch (Exception ex)
            {
                dbResult.IsError = true;
                dbResult.Message = ex.InnerException?.Message ?? ex.Message;

                _logService.Error(this, "Error Executing Query", ex.InnerException ?? ex);
            }

            return dbResult;
        }

        public virtual DbResult<IEnumerable<T>> Query<T>(string sql, IEnumerable<string> paramList, System.Data.CommandType commandType)
        {
            var ps = new DynamicParameters();

            foreach (var paramObj in paramList)
            {
                string[] paramDetails = paramObj.Split(':');
                switch (paramDetails[2])
                {
                    case "int16":
                    {
                        ps.Add(paramDetails[0], Convert.ToInt16(paramDetails[1]), System.Data.DbType.Int16, DirectionFinder(paramDetails[2]));
                        break;
                    }
                    case "int32":
                    {
                        ps.Add(paramDetails[0], Convert.ToInt32(paramDetails[1]), System.Data.DbType.Int32, DirectionFinder(paramDetails[2]));
                        break;
                    }
                    case "boolean":
                    {
                        ps.Add(paramDetails[0], Convert.ToBoolean(paramDetails[1]), System.Data.DbType.Boolean, DirectionFinder(paramDetails[2]));
                        break;
                    }
                    default:
                    {
                        ps.Add(paramDetails[0], Convert.ToString(paramDetails[1]), System.Data.DbType.String, DirectionFinder(paramDetails[2]));
                        break;
                    }
                }
            }
            
            var dbResult = new DbResult<IEnumerable<T>>();

            try
            {
                using (var connection = _connectionFactory.Create())
                {
                    dbResult.Result = connection.Query<T>(sql, ps, commandType: commandType);
                }
            }
            catch (Exception ex)
            {
                dbResult.IsError = true;
                dbResult.Message = ex.InnerException?.Message ?? ex.Message;

                _logService.Error(this, "Error Executing Query", ex.InnerException ?? ex);
            }

            return dbResult;
        }

        public virtual DbResult<dynamic> QueryMultiple(string sql, object param, Func<SqlMapper.GridReader, dynamic> func)
        {
            var dbResult = new DbResult<dynamic>();

            try
            {
                using (var connection = _connectionFactory.Create())
                {
                    using (var query = connection.QueryMultiple(sql, param))
                    {
                        dbResult.Result = func(query);
                    }
                }
            }
            catch (Exception ex)
            {
                dbResult.IsError = true;
                dbResult.Message = ex.InnerException?.Message ?? ex.Message;

                _logService.Error(this, "Error Executing QueryMultiple", ex.InnerException ?? ex);
            }

            return dbResult;
        }

        public virtual DbResult<int> Execute(string sql, object entity)
        {
            var dbResult = new DbResult<int>();

            try
            {
                using (var connection = _connectionFactory.Create())
                {
                    dbResult.Result = connection.Execute(sql, entity);
                }
            }
            catch (Exception ex)
            {
                dbResult.IsError = true;
                dbResult.Message =
                    $"SQL: {sql}. Message: {ex.InnerException?.Message ?? ex.Message}";

                _logService.Error(this, "Error Executing Execute", ex.InnerException ?? ex);
            }

            return dbResult;
        }

        private System.Data.ParameterDirection DirectionFinder(string direction)
        {
            return direction == "0" ? System.Data.ParameterDirection.Input : (direction == "1" ? System.Data.ParameterDirection.Output : System.Data.ParameterDirection.InputOutput);
        }

        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogService _logService;
    }
}