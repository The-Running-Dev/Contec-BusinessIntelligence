using System;
using System.Linq;
using System.Collections.Generic;

using Contec.Data.Models;
using Contec.Data.ViewModel;

namespace Contec.Data.Extensions
{
    public static class BIReportExtensions
    {
        public static BIReport ToModel(this BIReportViewModel viewModel)
        {
            return new BIReport()
            {
                Id = (viewModel.Id != Guid.Empty) ? viewModel.Id : Guid.NewGuid(),
                Name = viewModel.Name,
                Description = viewModel.Description,
                EmbedSource = viewModel.EmbedSource,
                CreatedBy = viewModel.CreatedBy
            };
        }

        public static IEnumerable<BIReport> ToModels(this IEnumerable<BIReportViewModel> listOfViewModels)
        {
            return listOfViewModels?.ToList().Select(item => item.ToModel()) ?? new List<BIReport>();
        }

        public static BIReportViewModel ToViewModel(this BIReport model)
        {
            return new BIReportViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                EmbedSource = model.EmbedSource,
                CreatedBy = model.CreatedBy
            };
        }

        public static IEnumerable<BIReportViewModel> ToViewModels(this IEnumerable<BIReport> entities)
        {
            return entities?.ToList().Select(item => item.ToViewModel()) ?? new List<BIReportViewModel>();
        }
    }
}