using System.Collections.Generic;

using Contec.Data.Models;
using Contec.Framework.Data;
using Contec.Framework.Models;
using Contec.Data.Connections;
using Contec.Framework.Logging;

namespace Contec.Data.Repositories
{
    public interface ISitesRepository
    {
        DbResult<IEnumerable<Site>> GetAll();
    }

    public class SitesRepository : BaseRepository, ISitesRepository
    {
        public SitesRepository(ICTGConnection connectionFactory, ILogService logService) : base((IConnectionFactory)connectionFactory, logService)
        {
            _logService = logService;
        }

        /// <summary>
        /// Gets all site records
        /// </summary>
        public DbResult<IEnumerable<Site>> GetAll()
        {
            return Query<Site>(_getAllSql, null);
        }

        private string _getAllSql = @"
select      SiteId, SiteName, ParentId
from        WorkSites
order by    SiteName
";

        private readonly ILogService _logService;
    }
}