using System;
using System.Web.Mvc;

using BI.Web.Controllers;
using Contec.Data.Services;
using Contec.Data.ViewModel;
using Contec.Data.Models.Actions;

namespace BI.Web.Areas.Admin.Controllers
{
    [Authorize]
    [RouteArea("admin")]
    public class ReportsController : BaseController
    {
        public ReportsController(IBIReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        [Route("reports")]
        public ActionResult GetAll()
        {
            var action = new GetAllAction(User.Identity.Name);
            var reports = _reportsService.GetAllReports(action);

            return View("List", reports);
        }

        [Route("reports/view/{id}")]
        public ActionResult View(Guid id)
        {
            var action = new GetByIdAction(id, User.Identity.Name);
            var report = _reportsService.GetById(action);

            return View("View", report);
        }

        [Route("reports/create")]
        public ActionResult Create()
        {
            var action = new EmptyAction(User.Identity.Name);
            var report = _reportsService.Create(action);

            return View("Form", report);
        }

        [HttpPost]
        [Route("reports/create")]
        public ActionResult Create(BIReportViewModel viewModel)
        {
            var action = new CreateAction(viewModel, User.Identity.Name);

            if (ModelState.IsValid)
            {
                _reportsService.Create(action);

                return RedirectToAction("");
            }

            //viewModel = _reportsService.Create(new EmptyAction(User.Identity.Name));
            //viewModel.AvailableSites = _reportsService.GetAllSites().ToSelectList((x => x.SiteId.ToString()), (x => x.SiteName));
            //viewModel.CreatedBy = User.Identity.Name;

            return View("Form", viewModel);
        }

        [Route("reports/edit/{id}")]
        public ActionResult Edit(Guid id)
        {
            var action = new GetByIdAction(id, User.Identity.Name);
            var report = _reportsService.GetById(action);

            if (report != null)
            {
                return View("Form", report);
            }

            return RedirectToAction("");
        }

        [HttpPost]
        [Route("reports/edit/{id}")]
        public ActionResult Edit(BIReportViewModel viewModel)
        {
            var updateAction = new UpdateAction(viewModel, User.Identity.Name);

            if (ModelState.IsValid)
            {
                _reportsService.Update(updateAction);

                return RedirectToAction("");
            }

            var getAction = new GetByIdAction(viewModel.Id, User.Identity.Name);
            var report = _reportsService.GetById(getAction);

            return View("Form", report);
        }

        [Route("reports/delete/{id}")]
        public ActionResult Delete(Guid id)
        {
            var action = new DeleteAction(id, User.Identity.Name);

            _reportsService.Delete(action);

            return RedirectToAction("");
        }

        private readonly IBIReportsService _reportsService;
    }
}