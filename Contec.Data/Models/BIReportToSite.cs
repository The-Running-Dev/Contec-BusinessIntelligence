using System;

namespace Contec.Data.Models
{
    public class BIReportToSite: DataModel
    {
        public int SiteId { get; set; }

        public Guid ReportId { get; set; }
    }
}