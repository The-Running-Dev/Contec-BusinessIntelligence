using System.Collections.Generic;

namespace Contec.Data.ViewModel
{
    public class BIReportsListViewModel
    {
        public IEnumerable<BIReportViewModel> Reports { get; set; }

        public BIReportsListViewModel()
        {
            Reports = new List<BIReportViewModel>();
        }
    }
}