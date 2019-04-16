using System.Collections.Generic;

using Contec.Data.Models;

namespace Contec.Data.ViewModel
{
    public class ReportsPerSiteViewModel
    {
        public List<int> AuthorizedSites { get; set; }

        public List<BIReport> Reports { get; set; }

        public ReportsPerSiteViewModel()
        {
            AuthorizedSites = new List<int>();
            Reports = new List<BIReport>();
        }
    }
}