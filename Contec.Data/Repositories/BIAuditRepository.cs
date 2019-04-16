using System;
using System.Collections.Generic;

using Contec.Framework.Data;
using Contec.Data.Connections;
using Contec.Framework.Models;
using Contec.Framework.Logging;
using Contec.Data.Models.Audits;

namespace Contec.Data.Repositories
{
    public interface IBIAuditRepository
    {
        DbResult<IEnumerable<IAudit>> GetAll();

        DbResult<IAudit> GetById(Guid id);

        void Create(IAudit entity);
    }

    /// <summary>
    /// Repository class to read/write database data related to repairs
    /// </summary>
    public class BIAuditRepository : BaseRepository, IBIAuditRepository
    {
        public BIAuditRepository(IBusinessIntelligenceConnection connectionFactory, ILogService logService) : base((IConnectionFactory)connectionFactory, logService)
        {
            _logService = logService;
        }

        /// <summary>
        /// Gets all BIAudit records
        /// </summary>
        public DbResult<IEnumerable<IAudit>> GetAll()
        {
            return Query<IAudit>(_getAllSql, null);
        }

        /// <summary>
        /// Gets a BIAudit record by its ID
        /// </summary>
        public DbResult<IAudit> GetById(Guid id)
        {
            return Single<IAudit>(_getByIdSql, new { Id = id} );
        }

        /// <summary>
        /// Creates a new BIAudit record
        /// </summary>
        public void Create(IAudit entity)
        {
            Execute(_createSql, entity);
        }
        
        private string _getAllSql = @"
select  Id, ActionId, Action, CreatedBy, CreatedOn
from    BIAudit
";

        private string _getByIdSql = @"
select  Id, ActionId, Action, CreatedBy, CreatedOn
from    BIAudit
where   Id = @Id
";

        private string _createSql = @"
insert
into    BIAudit (Id, ActionId, Action, CreatedBy, CreatedOn)
values  (@Id, @ActionId, @Action, @CreatedBy, @CreatedOn)
";

        private readonly ILogService _logService;
    }
}