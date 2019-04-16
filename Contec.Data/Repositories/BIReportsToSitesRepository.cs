using System;
using System.Collections.Generic;

using Contec.Data.Models;
using Contec.Framework.Data;
using Contec.Data.Connections;
using Contec.Framework.Logging;
using Contec.Framework.Models;

namespace Contec.Data.Repositories
{
    public interface IBIReportsToSitesRepository
    {
        DbResult<IEnumerable<BIReportToSite>> GetAll();

        DbResult<BIReportToSite> GetById(Guid id);

        void Create(BIReportToSite entity);

        void Update(BIReportToSite entity);

        void Delete(Guid id);

        void DeleteAllSiteForReport(Guid reportId);

        DbResult<IEnumerable<Site>> GetAllSiteForReport(Guid reortId);
    }

    /// <summary>
    /// Repository class to read/write database data related to repairs
    /// </summary>
    public class BIReportsToSitesRepository : BaseRepository, IBIReportsToSitesRepository
    {
        public BIReportsToSitesRepository(IBusinessIntelligenceConnection connectionFactory, ILogService logService) : base((IConnectionFactory)connectionFactory, logService)
        {
            _logService = logService;
        }

        /// <summary>
        /// Gets all BIReport records
        /// </summary>
        public DbResult<IEnumerable<BIReportToSite>> GetAll()
        {
            return Query<BIReportToSite>(_getAllSql, null);
        }

        /// <summary>
        /// Gets a BIReport record by its ID
        /// </summary>
        public DbResult<BIReportToSite> GetById(Guid id)
        {
            return Single<BIReportToSite>(_getByIdSql, new { Id = id} );
        }

        /// <summary>
        /// Creates a new BIReportToSite record
        /// </summary>
        public void Create(BIReportToSite entity)
        {
            Execute(_createSql, entity);
        }

        /// <summary>
        /// Updates a new BIReportToSite record
        /// </summary>
        public void Update(BIReportToSite entity)
        {
            Execute(_updateSql, entity);
        }

        /// <summary>
        /// Deletes a BIReportToSite record
        /// </summary>
        /// <param name="id">The ID of the record to delete</param>
        public void Delete(Guid id)
        {
            Execute(_deleteSql, new { Id = id });
        }

        /// <summary>
        /// Gets all BIReportToSite records for the given report ID
        /// </summary>
        /// <param name="reportId">The ID of the report to delete</param>
        public DbResult<IEnumerable<Site>> GetAllSiteForReport(Guid reportId)
        {
            return Query<Site>(_getAllSitesForReportSql, new { ReportId = reportId });
        }

        /// <summary>
        /// Deletes all BIReportToSite records for the given report ID
        /// </summary>
        /// <param name="reportId">The ID of the report to delete</param>
        public void DeleteAllSiteForReport(Guid reportId)
        {
            Execute(_deleteAllSitesForReportSql, new { ReportId = reportId });
        }

        private string _getAllSql = @"
select  Id, SiteId, ReportId, CreatedBy, CreatedOn
from    BIReportsToSites
";

        private string _getByIdSql = @"
select  Id, SiteId, ReportId, CreatedBy, CreatedOn
from    BIReportsToSites
where   Id = @Id
";

        private string _createSql = @"
insert
into    BIReportsToSites (Id, SiteId, ReportId, CreatedBy, CreatedOn)
values  (@Id, @SiteId, @ReportId, @CreatedBy, @CreatedOn)
";

        private string _updateSql = @"
update  BIReportsToSites
set     SiteId = @SiteId,
        ReportId = @ReportId
where   Id = @Id
";

        private string _deleteSql = @"
delete
from    BIReportsToSites
where   Id = @Id
";

        private string _getAllSitesForReportSql = @"
select  Id, SiteId, ReportId, CreatedBy, CreatedOn
from    BIReportsToSites
where   ReportId = @ReportId
";

        private string _deleteAllSitesForReportSql = @"
delete
from    BIReportsToSites
where   ReportId = @ReportId
";

        private readonly ILogService _logService;
    }
}