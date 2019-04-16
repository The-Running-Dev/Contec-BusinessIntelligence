using System;
using System.Collections.Generic;

using Dapper;

using Contec.Framework.Data;
using Contec.Framework.Models;
using Contec.Framework.Logging;

namespace Contec.Data.Repositories
{
    public class AuditedBaseRepository: BaseRepository
    {
        public AuditedBaseRepository(IConnectionFactory connectionFactory, ILogService logService): base(connectionFactory, logService)
        {
        }

        public override DbResult<T> Single<T>(string sql, object param)
        {
           return base.Single<T>(sql, param);
        }

        public override DbResult<IEnumerable<T>> Query<T>(string sql, object param)
        {
            return base.Query<T>(sql, param);
        }

        public override DbResult<IEnumerable<T>> Query<T>(string sql, IEnumerable<string> paramList, System.Data.CommandType commandType)
        {
            return base.Query<T>(sql, paramList, commandType);
        }

        public override DbResult<dynamic> QueryMultiple(string sql, object param, Func<SqlMapper.GridReader, dynamic> func)
        {
            return base.QueryMultiple(sql, param, func);
        }

        public override DbResult<int> Execute(string sql, object entity)
        {
            return base.Execute(sql, entity);
        }

        private IBIAuditRepository _auditRepository;
    }
}