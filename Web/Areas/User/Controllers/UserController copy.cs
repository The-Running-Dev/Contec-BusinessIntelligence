using System.Web.Mvc;

using BI.Web.Controllers;
using Contec.Data.Services;
using Contec.Data.Models.Actions;

namespace BI.Web.Areas.User.Controllers
{
    public class UserController : BaseController
    {
        public UserController(IBIReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        public ActionResult Index()
        {
            var action = new GetByIdsAction(User.Identity.Name);
            var viewModel = _reportsService.GetReportsBySiteId(action);

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