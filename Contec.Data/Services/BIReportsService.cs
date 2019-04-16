using System;
using System.Linq;
using System.Collections.Generic;

using Contec.Data.Models;
using Contec.Data.ViewModel;
using Contec.Data.Extensions;
using Contec.Data.Models.Actions;
using Contec.Data.Models.Audits;
using Contec.Data.Repositories;
using Contec.Framework.Extensions;

namespace Contec.Data.Services
{
    public interface IBIReportsService
    {
        /// <summary>
        /// Gets all BI reports
        /// </summary>
        /// <param name="action"></param>
        BIReportsListViewModel GetAllReports(GetAllAction action);

        /// <summary>
        /// Gets a BI report by its ID
        /// </summary>
        /// <param name="action"></param>
        BIReportViewModel GetById(GetByIdAction action);

        /// <summary>
        /// Creates a new empty BI report
        /// </summary>
        /// <param name="action"></param>
        BIReportViewModel Create(EmptyAction action);

        /// <summary>
        /// Persists a new BI report
        /// </summary>
        /// <param name="action"></param>
        void Create(CreateAction action);

        /// <summary>
        /// Updates an existing BI report
        /// </summary>
        /// <param name="action"></param>
        void Update(UpdateAction action);

        /// <summary>
        /// Deletes a BI report
        /// </summary>
        /// <param name="action"></param>
        void Delete(DeleteAction action);

        /// <summary>
        /// Gets the all reports for current user ID
        /// </summary>
        /// <param name="action"></param>
        ReportsPerSiteViewModel GetReportsBySiteId(GetByIdsAction action);
    }

    public class BIReportsService : IBIReportsService
    {
        public BIReportsService(
            IAccountService accountService,
            IBIReportsRepository biReportsRepository,
            IBIReportsToSitesRepository biReportsToSitesRepository,
            IBIAuditRepository biAuditRepository,
            ISitesRepository sitesRepository
        )
        {
            _accountService = accountService;
            _biReportsRepository = biReportsRepository;
            _biReportsToSitesRepository = biReportsToSitesRepository;
            _biAuditRepository = biAuditRepository;
            _sitesRepository = sitesRepository;
        }

        public BIReportsListViewModel GetAllReports(GetAllAction action)
        {
            var reports = _biReportsRepository.GetAll().Result;
            var reportToSites = _biReportsToSitesRepository.GetAll().Result;
            var sites = _sitesRepository.GetAll().Result;
            var viweModels = new List<BIReportViewModel>();

            reports?.ToList().ForEach(item =>
            {
                var viewModel = item.ToViewModel();

                viewModel.SiteIds = reportToSites.ToList()
                    .Where(reportToSite => reportToSite.ReportId == item.Id)
                    .Select(reportToSite => reportToSite.SiteId)
                    .ToList();

                viewModel.SiteNames = sites.ToList()
                    .Where(site => viewModel.SiteIds.Exists(siteId => siteId == site.SiteId))
                    .Select(site => site.SiteName)
                    .ToList();

                viweModels.Add(viewModel);
            });

            _biAuditRepository.Create(action.Audit);

            return new BIReportsListViewModel()
            {
                Reports = viweModels
            };
        }

        public BIReportViewModel GetById(GetByIdAction action)
        {
            if (action.Param != Guid.Empty)
            {
                var viewModel = _biReportsRepository.GetById(action.Param).Result.ToViewModel();

                viewModel.SiteIds = _biReportsToSitesRepository.GetAllSiteForReport(action.Param).Result.Select(item => item.SiteId).ToList();
                viewModel.AvailableSites = GetAllSites().ToSelectList(x => x.SiteId.ToString(), x => x.SiteName);

                _biAuditRepository.Create(action.Audit);

                return viewModel;
            }

            _biAuditRepository.Create(new NotFoundAudit(action.UserId));
            return null;
        }

        public BIReportViewModel Create(EmptyAction action)
        {
            _biAuditRepository.Create(action.Audit);
            var allSites = new List<Site> {new Site()};
            allSites.AddRange(GetAllSites());

            return new BIReportViewModel
            {
                AvailableSites = allSites.ToSelectList((x => x.SiteId.ToString()), (x => x.SiteName)),
                CreatedBy = action.UserId
            };
        }

        public void Create(CreateAction action)
        {
            var model = action.Param.ToModel();

            _biReportsRepository.Create(model);
            _biReportsToSitesRepository.DeleteAllSiteForReport(model.Id);

            action.Param.SiteIds.ForEach(item =>
            {
                _biReportsToSitesRepository.Create(
                    new BIReportToSite()
                    {
                        SiteId = item,
                        ReportId = model.Id,
                        CreatedBy = action.UserId
                    }
                );
            });

            _biAuditRepository.Create(action.Audit);
        }

        public void Update(UpdateAction action)
        {
            var model = action.Param.ToModel();

            _biReportsRepository.Update(model);
            _biReportsToSitesRepository.DeleteAllSiteForReport(model.Id);

            action.Param.SiteIds.ForEach(item =>
            {
                _biReportsToSitesRepository.Create(
                    new BIReportToSite()
                    {
                        SiteId = item,
                        ReportId = model.Id,
                        CreatedBy = action.UserId
                    }
                );
            });

            _biAuditRepository.Create(action.Audit);
        }

        public void Delete(DeleteAction action)
        {
            if (action.Param != Guid.Empty)
            {
                _biReportsToSitesRepository.DeleteAllSiteForReport(action.Param);
                _biReportsRepository.Delete(action.Param);

                _biAuditRepository.Create(action.Audit);
            }
            else
            {
                _biAuditRepository.Create(new NotFoundAudit(action.UserId));
            }
        }

        public ReportsPerSiteViewModel GetReportsBySiteId(GetByIdsAction action)
        {
            var authorizedSites = _accountService.GetAuthorizedSites(action.UserId);
            var dbResults = _biReportsRepository.GetBySiteId(authorizedSites);
            var reports = new List<BIReport>();

            if (dbResults.IsError)
            {
                _biAuditRepository.Create(new ErrorAudit(action.UserId));
            }
            else
            {
                reports = dbResults.Result.ToList();
            }

            _biAuditRepository.Create(action.Audit);

            return new ReportsPerSiteViewModel()
            {
                AuthorizedSites = authorizedSites,
                Reports = reports
            };
        }

        private IEnumerable<Site> GetAllSites()
        {
            return _sitesRepository.GetAll().Result;
        }

        private readonly IAccountService _accountService;
        private readonly IBIReportsRepository _biReportsRepository;
        private readonly IBIReportsToSitesRepository _biReportsToSitesRepository;
        private readonly IBIAuditRepository _biAuditRepository;
        private readonly ISitesRepository _sitesRepository;
    }
}