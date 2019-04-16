using System;
using System.Collections.Generic;

using Contec.Data.Models;
using Contec.Framework.Data;
using Contec.Framework.Models;
using Contec.Data.Connections;
using Contec.Framework.Logging;

namespace Contec.Data.Repositories
{
    public interface IBIReportsRepository
    {
        DbResult<IEnumerable<BIReport>> GetAll();

        DbResult<BIReport> GetById(Guid id);

        void Create(BIReport entity);

        void Update(BIReport entity);

        void Delete(Guid id);

        DbResult<IEnumerable<BIReport>> GetBySiteId(List<int> siteIds);
    }

    /// <summary>
    /// Repository class to read/write database data related to repairs
    /// </summary>
    public class BIReportsRepository : BaseRepository, IBIReportsRepository
    {
        public BIReportsRepository(IBusinessIntelligenceConnection connectionFactory, ILogService logService) : base((IConnectionFactory)connectionFactory, logService)
        {
            _logService = logService;
        }

        /// <summary>
        /// Gets all BIReport records
        /// </summary>
        public DbResult<IEnumerable<BIReport>> GetAll()
        {
            return Query<BIReport>(_getAllSql, null);
        }

        /// <summary>
        /// Gets a BIReport record by its ID
        /// </summary>
        public DbResult<BIReport> GetById(Guid id)
        {
            return Single<BIReport>(_getByIdSql, new { Id = id });
        }

        /// <summary>
        /// Creates a new BIReport record
        /// </summary>
        public void Create(BIReport entity)
        {
            Execute(_createSql, entity);
        }

        /// <summary>
        /// Updates a new BIReport record
        /// </summary>
        public void Update(BIReport entity)
        {
            Execute(_updateSql, entity);
        }

        /// <summary>
        /// Deletes a BIReport record
        /// </summary>
        /// <param name="id">The ID of the record to delete</param>
        public void Delete(Guid id)
        {
            Execute(_deleteSql, new { Id = id });
        }

        /// <summary>
        /// Gets a BIReport record by the site ID
        /// </summary>
        public DbResult<IEnumerable<BIReport>> GetBySiteId(List<int> siteIds)
        {
            return Query<BIReport>(_getBySiteIdSql, new { SiteIds = siteIds });
        }

        private string _getAllSql = @"
select  a.Id, a.Name, a.Description, a.EmbedSource, a.CreatedBy, a.CreatedOn
from    BIReports as a
";

        private string _getByIdSql = @"
select  Id, Name, Description, EmbedSource, CreatedBy, CreatedOn
from    BIReports
where   Id = @Id
";

        private string _getBySiteIdSql = @"
select  a.Id, a.Name, a.Description, a.EmbedSource, a.CreatedBy, a.CreatedOn
from    BIReports as a
        inner join
        BIReportsToSites as b
        on a.Id = b.ReportId
where   b.SiteId in @siteIds
";

        private string _createSql = @"
insert
into    BIReports (Id, Name, Description, EmbedSource, CreatedBy, CreatedOn)
values  (@Id, @Name, @Description, @EmbedSource, @CreatedBy, @CreatedOn);
select  last_insert
";

        private string _updateSql = @"
update  BIReports
set     Name = @Name,
        Description = @Description,
        EmbedSource = @EmbedSource
where   Id = @Id
";

        private string _deleteSql = @"
delete
from    BIReports
where   Id = @Id
";

        private readonly ILogService _logService;
    }
}