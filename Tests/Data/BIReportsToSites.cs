using System;
using System.Collections.Generic;

using Contec.Data.Models;

namespace BI.Web.Tests.Data
{
    public static class BIReportsToSites
    {
        public static List<BIReportToSite> Data;

        static BIReportsToSites()
        {
            Data = new List<BIReportToSite>();

            for (var i = 1; i < 11; i++)
            {
                Data.Add(New(Guid.NewGuid()));
            }
        }

        public static BIReportToSite New(Guid id)
        {
            return new BIReportToSite()
            {
                Id = id,
                SiteId = 1,
                ReportId = Guid.Empty,
                CreatedBy = "TestUser",
                CreatedOn = DateTime.Now.ToUniversalTime()
            };
        }
    }
}