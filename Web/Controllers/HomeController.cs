using System.Web.Mvc;

using Contec.Data.Services;
using Contec.Data.ViewModel;
using Contec.Data.Models.Actions;

namespace BI.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IBIReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        public ActionResult Index()
        {
            var viewModel = new ReportsPerSiteViewModel();

            if (Request.IsAuthenticated)
            {
                var action = new GetByIdsAction(User.Identity.Name);
                viewModel = _reportsService.GetReportsBySiteId(action);
            }

            return View(viewModel);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        private readonly IBIReportsService _reportsService;
    }
}